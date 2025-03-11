resource "random_password" "metabase_embedding_key" {
  length  = 64
  special = false
}

resource "kubernetes_secret_v1" "exampleapp_metabase_secret" {
  metadata {
    name      = "exampleapp-metabase-secret"
    namespace = data.kubernetes_namespace_v1.main.metadata[0].name
    labels    = local.labels_metabase
  }

  data = {
    "MB_DB_TYPE"              = "postgres",
    "MB_DB_DBNAME"            = "metabase"
    "MB_DB_PORT"              = "5432"
    "MB_DB_USER"              = module.postgresql.roles["metabase"].name,
    "MB_DB_PASS"              = module.postgresql.roles["metabase"].password,
    "MB_DB_HOST"              = module.postgresql.server_fqdn,
    "MB_EMBEDDING_SECRET_KEY" = random_password.metabase_embedding_key.result,
    "MB_ENABLE_EMBEDDING"     = true,
  }
}

resource "kubernetes_deployment_v1" "exampleapp_metabase" {
  metadata {
    name      = "exampleapp-metabase"
    namespace = data.kubernetes_namespace_v1.main.metadata[0].name
    labels    = local.labels_metabase
  }
  spec {
    replicas = 1
    selector {
      match_labels = local.labels_metabase
    }
    template {
      metadata {
        labels = local.labels_metabase
      }
      spec {
        container {
          name  = "metabase"
          image = "docker.io/metabase/metabase:v0.47.5"

          env_from {
            secret_ref {
              name = kubernetes_secret_v1.exampleapp_metabase_secret.metadata[0].name
            }
          }

          resources {
            requests = {
              cpu    = "100m"
              memory = "250Mi"
            }
            limits = {
              cpu    = "200m"
              memory = "1Gi"
            }
          }
        }
      }
    }
  }
}

resource "kubernetes_service_v1" "metabase_service" {
  metadata {
    name      = "exampleapp-metabase-svc"
    namespace = data.kubernetes_namespace_v1.main.metadata[0].name
    labels    = local.labels_metabase
  }
  spec {
    type     = "ClusterIP"
    selector = local.labels_metabase
    port {
      name        = "public"
      port        = 80
      target_port = 3000
    }
  }
}
resource "kubernetes_ingress_v1" "metabase_ingress" {
  metadata {
    name      = "exampleapp-metabase-ingress"
    namespace = data.kubernetes_namespace_v1.main.metadata[0].name
    labels    = local.labels_metabase
  }
  spec {
    rule {
      host = "metabase.${var.domain}"
      http {
        path {
          backend {
            service {
              name = kubernetes_service_v1.metabase_service.metadata[0].name
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

locals {
  labels_metabase = merge(local.tags, { component = "metabase" })
}

output "metabase_roles_script" {
  value = <<EOT
      \connect "examples"

      grant "${local.databases["examples"].ad_roles.migrations_role}" to "${module.postgresql.administrator_login}";

      alter default privileges for role "${local.databases["examples"].ad_roles.migrations_role}"
      grant select
      on tables
      to "${module.postgresql.roles["metabase"].name}";

      grant select on all tables in schema public to "${module.postgresql.roles["metabase"].name}";

      alter default privileges for role "${local.databases["examples"].ad_roles.migrations_role}"
      grant usage
      on schemas
      to "${module.postgresql.roles["metabase"].name}";

      revoke "${local.databases["examples"].ad_roles.migrations_role}" from "${module.postgresql.administrator_login}";
    EOT
}
