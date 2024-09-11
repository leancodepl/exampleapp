resource "kubernetes_deployment_v1" "metabase" {
  count = var.optional_features.metabase ? 1 : 0

  metadata {
    name      = "exampleapp-metabase"
    namespace = kubernetes_namespace_v1.metabase.metadata[0].name
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
            value = postgresql_role.metabase.name
          }
          env {
            name  = "MB_DB_PASS"
            value = postgresql_role.metabase.password
          }
          env {
            name  = "MB_DB_HOST"
            value = "${kubernetes_service_v1.postgresql_service.metadata[0].name}.${kubernetes_service_v1.postgresql_service.metadata[0].namespace}.svc.cluster.local"
          }
          env {
            name  = "MB_EMBEDDING_SECRET_KEY"
            value = "embedding_secret_key_that_needs_to_have_256_bits"
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
  count = var.optional_features.metabase ? 1 : 0

  metadata {
    name      = "exampleapp-metabase-svc"
    namespace = kubernetes_namespace_v1.metabase.metadata[0].name
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
  count = var.optional_features.metabase ? 1 : 0

  metadata {
    name      = "exampleapp-metabase-ingress"
    namespace = kubernetes_namespace_v1.metabase.metadata[0].name
    labels    = local.labels_metabase
  }
  spec {
    rule {
      host = "metabase.local.lncd.pl"
      http {
        path {
          backend {
            service {
              name = kubernetes_service_v1.metabase_service[0].metadata[0].name
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
  labels_metabase = {
    project   = "exampleapp"
    component = "metabase"
  }
}
