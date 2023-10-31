resource "random_password" "metabase_embedding_key" {
  length  = 64
  special = false
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

          env {
            name  = "MB_DB_TYPE"
            value = "postgres"
          }
          env {
            name  = "MB_DB_DBNAME"
            value = "metabase"
          }
          env {
            name  = "MB_DB_PORT"
            value = "5432"
          }
          env {
            name  = "MB_DB_USER"
            value = module.postgresql.roles["metabase"].name
          }
          env {
            name  = "MB_DB_PASS"
            value = module.postgresql.roles["metabase"].password
          }
          env {
            name  = "MB_DB_HOST"
            value = module.postgresql.server_fqdn
          }
          env {
            name  = "MB_EMBEDDING_SECRET_KEY"
            value = random_password.metabase_embedding_key.result
          }
          env {
            name  = "MB_ENABLE_EMBEDDING"
            value = true
          }

          resources {
            requests = {
              cpu    = "250m"
              memory = "1Gi"
            }
            limits = {
              cpu    = "500m"
              memory = "2Gi"
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
