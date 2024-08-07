module "kratos" {
  source     = "git::https://github.com/leancodepl/terraform-kratos-module.git//kratos?ref=5f5d2a06cd9d8d314b1f7b75ea1fa0c323e7b5fd"
  depends_on = [kubernetes_namespace_v1.kratos, postgresql_grant.kratos, postgresql_grant.kratos_public]

  namespace    = kubernetes_namespace_v1.kratos.metadata[0].name
  project      = "exampleapp"
  ingress_host = "auth.local.lncd.pl"
  image        = "leancodepublic.azurecr.io/kratos:v1.2.0-3-gdcc4a5c5"
  replicas     = 1

  resources = {
    requests = {
      cpu    = "100m"
      memory = "128Mi"
    }
    limits = {
      cpu    = "100m"
      memory = "128Mi"
    }
  }

  courier_resources = {
    requests = {
      cpu    = "100m"
      memory = "128Mi"
    }
    limits = {
      cpu    = "100m"
      memory = "128Mi"
    }
  }

  config_files = {
    for f in fileset("./kratos", "*") : f => file("./kratos/${f}")
  }

  config_yaml = templatefile("./kratos.yaml", {
    api                 = "http://exampleapp-examples-api-svc.exampleapp-dev.svc.cluster.local"
    domain              = "local.lncd.pl"
    oidc_config         = var.oidc_config
    authority_name      = "ExampleApp (dev)"
    web_hook_api_key    = "Passw12#"
    passkey_origins     = var.passkey_origins
    dev_allowed_origins = []
  })

  dsn = "postgresql://${urlencode(postgresql_role.kratos.name)}:${urlencode(postgresql_role.kratos.password)}@${kubernetes_service_v1.postgresql_service.metadata[0].name}.${kubernetes_service_v1.postgresql_service.metadata[0].namespace}.svc.cluster.local/${postgresql_database.kratos.name}?sslmode=disable"

  courier_smtp_connection_uri = "smtps://apikey:${var.sendgrid_api_key}@smtp.sendgrid.net:465"
}

locals {
  labels_ui = {
    project   = "exampleapp"
    component = "kratos-ui"
  }
}

resource "random_password" "kratos_ui_csrf_cookie_secret" {
  length  = 32
  special = false
}

resource "kubernetes_secret_v1" "kratos_ui_secret" {
  metadata {
    name      = "exampleapp-kratos-ui-secret"
    namespace = kubernetes_namespace_v1.kratos.metadata[0].name
    labels    = local.labels_ui
  }

  data = {
    "CSRF_COOKIE_NAME"   = "__HOST-local.lncd.pl-x-csrf-token"
    "CSRF_COOKIE_SECRET" = random_password.kratos_ui_csrf_cookie_secret.result
  }
}

resource "kubernetes_deployment_v1" "kratos_ui" {
  metadata {
    name      = "exampleapp-kratos-ui"
    namespace = kubernetes_namespace_v1.kratos.metadata[0].name
    labels    = local.labels_ui
  }
  spec {
    replicas = 1
    selector {
      match_labels = local.labels_ui
    }
    template {
      metadata {
        labels = local.labels_ui
      }
      spec {
        container {
          name  = "kratos-ui"
          image = "docker.io/oryd/kratos-selfservice-ui-node:v1.2.0"
          env_from {
            secret_ref {
              name = kubernetes_secret_v1.kratos_ui_secret.metadata[0].name
            }
          }
          env {
            name  = "KRATOS_PUBLIC_URL"
            value = module.kratos.internal_service_url.public
          }
          env {
            name  = "KRATOS_BROWSER_URL"
            value = module.kratos.external_ingress_url
          }
          port {
            name           = "public"
            container_port = "3000"
          }
          resources {
            requests = {
              cpu    = "100m"
              memory = "128Mi"
            }
            limits = {
              cpu    = "250m"
              memory = "256Mi"
            }
          }
        }
      }
    }
  }
}

resource "kubernetes_service_v1" "kratos_ui_service" {
  metadata {
    name      = "exampleapp-kratos-ui-svc"
    namespace = kubernetes_namespace_v1.kratos.metadata[0].name
    labels    = local.labels_ui
  }
  spec {
    type     = "ClusterIP"
    selector = local.labels_ui
    port {
      name        = "public"
      port        = 80
      target_port = 3000
    }
  }
}

resource "kubernetes_ingress_v1" "kratos_ui_ingress" {
  metadata {
    name      = "exampleapp-kratos-ui-ingress"
    namespace = kubernetes_namespace_v1.kratos.metadata[0].name
    labels    = local.labels_ui
  }
  spec {
    rule {
      host = "kratos-ui.local.lncd.pl"
      http {
        path {
          backend {
            service {
              name = kubernetes_service_v1.kratos_ui_service.metadata[0].name
              port {
                number = 80
              }
            }
          }
          path_type = "ImplementationSpecific"
        }
      }
    }
  }

  lifecycle {
    ignore_changes = [
      spec[0].ingress_class_name
    ]
  }
}
