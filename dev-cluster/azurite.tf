locals {
  azurite_account = "azurite"
  azurite_key     = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="
}

resource "kubernetes_deployment_v1" "azurite_deployment" {
  metadata {
    name      = "azurite"
    namespace = local.k8s_shared_namespace
    labels = {
      app = "azurite"
    }
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "azurite"
      }
    }
    template {
      metadata {
        labels = {
          app = "azurite"
        }
      }
      spec {
        container {
          command = [
            "azurite",
            "--skipApiVersionCheck",
            "--blobHost",
            "0.0.0.0",
            "--tableHost",
            "0.0.0.0",
            "-d",
            "/var/azurite.log"
          ]
          env {
            name  = "AZURITE_ACCOUNTS"
            value = "${local.azurite_account}:${local.azurite_key}"
          }
          image = "mcr.microsoft.com/azure-storage/azurite:3.33.0"
          name  = "azurite"
          port {
            container_port = 10000
          }
          port {
            container_port = 10001
          }
          port {
            container_port = 10002
          }
          volume_mount {
            name       = "data"
            mount_path = "/data"
          }
        }
        volume {
          name = "data"
          persistent_volume_claim {
            claim_name = kubernetes_manifest.azurite_pvc.manifest.metadata.name
          }
        }
      }
    }
  }
}

resource "kubernetes_manifest" "azurite_pvc" {
  manifest = {
    "apiVersion" = "v1"
    "kind"       = "PersistentVolumeClaim"
    "metadata" = {
      "name"      = "azurite-pvc"
      "namespace" = local.k8s_shared_namespace
      "labels" = {
        "app" = "azurite"
      }
    }
    "spec" = {
      "accessModes" = [
        "ReadWriteOnce",
      ]
      "resources" = {
        "requests" = {
          "storage" = "10Gi"
        }
      }
    }
  }
}

resource "kubernetes_service_v1" "azurite_service" {
  metadata {
    # keep the name in sync with AZURITE_ACCOUNTS, otherwise you get 400 (Invalid storage account.) when calling storage
    # using cluster local network
    name      = "azurite"
    namespace = local.k8s_shared_namespace
    labels = {
      app = "azurite"
    }
  }
  spec {
    type = "ClusterIP"
    port {
      port        = 80
      target_port = 10000
      name        = "blob"
    }
    # Queue is disabled by default on Azurite side
    port {
      port        = 81
      target_port = 10001
      name        = "queue"
    }
    port {
      port        = 82
      target_port = 10002
      name        = "table"
    }
    selector = {
      app = "azurite"
    }
  }
}

resource "kubernetes_ingress_v1" "azurite_ingress" {
  metadata {
    name      = "azurite-ingress"
    namespace = local.k8s_shared_namespace
    labels = {
      app = "azurite"
    }
  }
  spec {
    rule {
      host = "${local.azurite_account}.blob.local.lncd.pl"
      http {
        path {
          backend {
            service {
              name = kubernetes_service_v1.azurite_service.metadata[0].name
              port {
                name = "blob"
              }
            }
          }
          path_type = "ImplementationSpecific"
        }
      }
    }
    rule {
      host = "${local.azurite_account}.queue.local.lncd.pl"
      http {
        path {
          backend {
            service {
              name = kubernetes_service_v1.azurite_service.metadata[0].name
              port {
                name = "queue"
              }
            }
          }
          path_type = "ImplementationSpecific"
        }
      }
    }
    rule {
      host = "${local.azurite_account}.table.local.lncd.pl"
      http {
        path {
          backend {
            service {
              name = kubernetes_service_v1.azurite_service.metadata[0].name
              port {
                name = "table"
              }
            }
          }
          path_type = "ImplementationSpecific"
        }
      }
    }
  }
}

resource "null_resource" "audit_resources" {
  provisioner "local-exec" {
    command = <<-CMD
      az storage table create \
        -n audit \
        --account-name '${local.azurite_account}' --account-key '${local.azurite_key}' \
        --table-endpoint 'https://azurite.table.local.lncd.pl/'
      az storage container create \
        -n audit \
        --account-name '${local.azurite_account}' --account-key '${local.azurite_key}' \
        --blob-endpoint 'https://azurite.blob.local.lncd.pl/'
    CMD
  }

  depends_on = [
    kubernetes_deployment_v1.azurite_deployment,
    kubernetes_ingress_v1.azurite_ingress,
    helm_release.traefik,
  ]
}
