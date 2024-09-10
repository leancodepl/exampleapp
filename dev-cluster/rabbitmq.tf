resource "helm_release" "rabbit" {
  count = var.optional_features.rabbit ? 1 : 0

  name      = "rabbit"
  namespace = local.k8s_shared_namespace

  chart      = "rabbitmq"
  repository = "https://charts.bitnami.com/bitnami"

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
}

resource "kubernetes_manifest" "rabbit_ingress" {
  count = var.optional_features.rabbit ? 1 : 0

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
