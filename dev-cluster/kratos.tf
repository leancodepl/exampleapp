module "kratos" {
  source     = "git::https://github.com/leancodepl/terraform-kratos-module.git//kratos?ref=v0.2.0"
  depends_on = [kubernetes_namespace_v1.kratos, postgresql_grant.kratos, postgresql_grant.kratos_public]

  namespace    = kubernetes_namespace_v1.kratos.metadata[0].name
  project      = "exampleapp"
  ingress_host = "auth.local.lncd.pl"
  image        = "leancodepublic.azurecr.io/kratos:v1.3.1-4-g4c97f5058"
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
  count = var.optional_features.kratos_ui ? 1 : 0

  length  = 32
  special = false
}

resource "kubernetes_secret_v1" "kratos_ui_secret" {
  count = var.optional_features.kratos_ui ? 1 : 0

  metadata {
    name      = "exampleapp-kratos-ui-secret"
    namespace = kubernetes_namespace_v1.kratos.metadata[0].name
    labels    = local.labels_ui
  }

  data = {
    "CSRF_COOKIE_NAME"   = "__HOST-local.lncd.pl-x-csrf-token"
    "CSRF_COOKIE_SECRET" = random_password.kratos_ui_csrf_cookie_secret[0].result
  }
}

resource "kubernetes_ingress_v1" "kratos_admin_ingress" {
  metadata {
    name      = "exampleapp-kratos-admin-ingress"
    namespace = kubernetes_namespace_v1.kratos.metadata[0].name
  }
  spec {
    rule {
      host = "kratos-admin.local.lncd.pl"
      http {
        path {
          backend {
            service {
              name = module.kratos.service_name
              port {
                name = "admin"
              }
            }
          }
          path_type = "ImplementationSpecific"
        }
      }
    }
  }
}

resource "kubernetes_deployment_v1" "kratos_ui" {
  count = var.optional_features.kratos_ui ? 1 : 0

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
          image = "docker.io/oryd/kratos-selfservice-ui-node:v1.3.1"
          env_from {
            secret_ref {
              name = kubernetes_secret_v1.kratos_ui_secret[0].metadata[0].name
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
  count = var.optional_features.kratos_ui ? 1 : 0

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
  count = var.optional_features.kratos_ui ? 1 : 0

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
              name = kubernetes_service_v1.kratos_ui_service[0].metadata[0].name
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
