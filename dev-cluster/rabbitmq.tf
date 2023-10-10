locals {
  rabbit_image_name = "${local.registry_address}/rabbit"
  rabbit_full_name  = "${local.rabbit_image_name}:latest"
}

resource "docker_image" "rabbit" {
  count = var.rabbit ? 1 : 0

  name = local.rabbit_full_name

  build {
    context    = "./apps"
    dockerfile = "Dockerfile.rabbitmq"
    build_arg  = {
      dockerfile_trigger = filemd5("./apps/Dockerfile.rabbitmq")
    }
  }

  # Docker provider cannot push to insecure registries
  provisioner "local-exec" {
    command = "docker push ${local.rabbit_full_name}"
  }

  depends_on = [
    null_resource.cluster
  ]
}

resource "helm_release" "rabbit" {
  count = var.rabbit ? 1 : 0

  name      = "rabbit"
  namespace = local.k8s_shared_namespace

  chart      = "rabbitmq"
  repository = "https://charts.bitnami.com/bitnami"

  set {
    name  = "image.registry"
    value = local.registry_address
  }

  set {
    name  = "image.repository"
    value = "rabbit"
  }

  set {
    name  = "image.tag"
    value = "latest"
  }

  set {
    name  = "auth.username"
    value = "user"
  }

  set {
    name  = "auth.password"
    value = "user"
  }
  set {
    name  = "service.type"
    value = "ClusterIP"
  }

  set {
    # setting this is required for release updates
    name  = "auth.erlangCookie"
    value = "AZ6s6VSejs7Fgicj1G8mORMcnVwscVnN"
  }

  depends_on = [
    docker_image.rabbit[0]
  ]
}

resource "kubernetes_manifest" "rabbit_ingress" {
  count = var.rabbit ? 1 : 0

  manifest = {
    "apiVersion" = "networking.k8s.io/v1"
    "kind"       = "Ingress"
    "metadata"   = {
      "name"      = "rabbit-ingress"
      "namespace" = local.k8s_shared_namespace
      "labels"    = {
        "app" = "rabbit"
      }
    }
    "spec" = {
      "rules" = [
        {
          "host" = "rabbit.local.lncd.pl",
          "http" = {
            "paths" = [
              {
                "backend" = {
                  "service" = {
                    "name" = "rabbit-rabbitmq"
                    "port" = {
                      "number" = 15672
                    }
                  }
                }
                "pathType" = "ImplementationSpecific"
              }
            ]
          }
        }
      ]
    }
  }
}
