locals {
  labels_ui = merge(local.tags, { component = "kratos-ui" })
}

resource "kubernetes_deployment_v1" "kratos_ui" {
  metadata {
    name      = "${local.project}-kratos-ui"
    namespace = data.kubernetes_namespace_v1.main.metadata[0].name
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
          image = "docker.io/oryd/kratos-selfservice-ui-node:v1.0.0"
          env {
            name  = "KRATOS_PUBLIC_URL"
            value = module.kratos.internal_service_url.public
          }
          env {
            name  = "KRATOS_BROWSER_URL"
            value = "https://auth.${var.domain}"
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

resource "kubernetes_service_v1" "kratos_ui" {
  metadata {
    name      = "${local.project}-kratos-ui-svc"
    namespace = data.kubernetes_namespace_v1.main.metadata[0].name
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
