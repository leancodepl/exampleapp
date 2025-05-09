resource "kubernetes_deployment_v1" "aspire_deployment" {
  metadata {
    name      = "aspire"
    namespace = local.k8s_shared_namespace
    labels = {
      app = "aspire"
    }
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "aspire"
      }
    }
    template {
      metadata {
        labels = {
          app = "aspire"
        }
      }
      spec {
        container {
          env {
            name  = "DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS"
            value = "true"
          }
          env {
            name  = "DOTNET_DASHBOARD_OTLP_ENDPOINT_URL"
            value = "http://[::]:4317"
          }
          env {
            name  = "DOTNET_DASHBOARD_OTLP_HTTP_ENDPOINT_URL"
            value = "http://[::]:4318"
          }
          image = "mcr.microsoft.com/dotnet/aspire-dashboard"
          name  = "aspire"
          port {
            container_port = "18888"
          }
        }
      }
    }
  }
}

resource "kubernetes_service_v1" "aspire_service" {
  metadata {
    name      = "aspire-svc"
    namespace = local.k8s_shared_namespace
    labels = {
      app = "aspire"
    }
  }
  spec {
    selector = {
      app = "aspire"
    }
    port {
      name        = "dashboard"
      port        = 80
      target_port = 18888
    }
    port {
      name        = "otlp-grpc"
      port        = 4317
      target_port = 4317
    }
  }
}

resource "kubernetes_ingress_v1" "aspire_ingress" {
  metadata {
    name      = "aspire-ingress"
    namespace = local.k8s_shared_namespace
    labels = {
      app = "aspire"
    }
  }
  spec {
    rule {
      host = "aspire.local.lncd.pl"
      http {
        path {
          backend {
            service {
              name = kubernetes_service_v1.aspire_service.metadata[0].name
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
}
