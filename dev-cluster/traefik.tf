locals {
  traefik_image_name    = "${local.registry_address}/traefik"
  traefik_image_version = "2.6.0"
  traefik_image         = "${local.traefik_image_name}:${local.traefik_image_version}"

  traefik_triggers = {
    dockerfile_trigger   = filemd5(var.traefik_self_signed ? "./apps/Dockerfile.traefik-self-signed" : "./apps/Dockerfile.traefik")
    dynamic_toml_trigger = filemd5("./apps/dynamic.toml")
  }
}

resource "docker_image" "alpine" {
  name         = "docker.io/library/alpine:latest"
  keep_locally = true
}

resource "docker_container" "certificates" {
  image = docker_image.alpine.image_id
  name  = "exampleapp-certificates"

  start       = true
  attach      = true
  wait        = true
  rm          = true
  working_dir = "/mnt"
  command     = ["./generate_certs.sh"]

  mounts {
    type   = "bind"
    source = abspath("${path.module}/apps")
    target = "/mnt"
  }
}

resource "docker_image" "traefik" {
  name = local.traefik_image

  build {
    context    = "./apps"
    dockerfile = var.traefik_self_signed ? "Dockerfile.traefik-self-signed" : "Dockerfile.traefik"
  }

  triggers = local.traefik_triggers

  depends_on = [docker_container.certificates]
}

resource "docker_registry_image" "traefik" {
  name                 = docker_image.traefik.name
  insecure_skip_verify = true
  keep_remotely        = true

  triggers = local.traefik_triggers

  depends_on = [null_resource.cluster_kubeconfig]
}

resource "helm_release" "traefik" {
  chart      = "traefik/traefik"
  repository = "helm.traefik.io/traefik"
  version    = "10.13.0"

  name      = "traefik"
  namespace = local.k8s_shared_namespace

  set {
    name  = "image.name"
    value = local.traefik_image_name
  }

  set {
    name  = "image.tag"
    value = local.traefik_image_version
  }

  set {
    name  = "image.pullPolicy"
    value = "Always"
  }

  set {
    name  = "providers.kubernetesIngress.publishedService.enabled"
    value = true
  }

  set {
    name  = "ports.web.redirectTo"
    value = "websecure"
  }
  set {

    name  = "ports.websecure.tls.enabled"
    value = true
  }

  values = [yamlencode({
    globalArguments = [
      "--global.checkNewVersion=true",
      "--global.sendAnonymousUsage=false",
    ],

    additionalArguments = [
      "--providers.file.directory=/config/dynamic",
      "--log.level=DEBUG",
      "--api.insecure=true",
      "--api.debug=true",
      "--accesslog=true",
    ],
  })]

  depends_on = [
    docker_registry_image.traefik
  ]
}
