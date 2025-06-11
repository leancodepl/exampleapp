locals {
  traefik_image_version = "3.3.3"
  traefik_image         = "${local.registry_address}/traefik:${local.traefik_image_version}"

  traefik_triggers = {
    dockerfile_trigger   = filemd5("./apps/Dockerfile.traefik")
    dynamic_toml_trigger = filemd5("./apps/dynamic.toml")
  }
}

resource "docker_image" "traefik" {
  name = local.traefik_image

  build {
    context    = "./apps"
    dockerfile = "Dockerfile.traefik"
  }

  triggers = local.traefik_triggers
}

resource "docker_registry_image" "traefik" {
  name                 = docker_image.traefik.name
  insecure_skip_verify = true
  keep_remotely        = true

  triggers = local.traefik_triggers

  depends_on = [null_resource.cluster_kubeconfig]

  lifecycle {
    replace_triggered_by = [docker_image.traefik]
  }
}

resource "helm_release" "traefik" {
  chart      = "traefik/traefik"
  repository = "helm.traefik.io/traefik"
  version    = "31.0.0"

  name      = "traefik"
  namespace = local.k8s_shared_namespace

  set {
    name  = "image.registry"
    value = local.registry_address
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
    name  = "logs.general.level"
    value = "DEBUG"
  }

  set {
    name  = "ingressRoute.dashboard.enabled"
    value = true
  }

  set {
    name  = "ingressRoute.dashboard.matchRule"
    value = "Host(`traefik.local.lncd.pl`) && (PathPrefix(`/dashboard`) || PathPrefix(`/api`))"
  }

  set {
    name  = "ingressRoute.dashboard.entryPoints[0]"
    value = "web"
  }

  set {
    name  = "ingressRoute.dashboard.entryPoints[1]"
    value = "websecure"
  }

  set {
    name  = "metrics.prometheus"
    value = "null"
  }

  set {
    name  = "providers.file.enabled"
    value = true
  }

  values = [yamlencode({
    additionalArguments = [
      "--providers.file.directory=/config/dynamic",
      "--entryPoints.web.http.redirections.entryPoint.to=:443"
    ],
  })]

  depends_on = [
    docker_registry_image.traefik
  ]

  lifecycle {
    replace_triggered_by = [docker_registry_image.traefik]
  }
}
