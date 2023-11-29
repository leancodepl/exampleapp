locals {
  kratos_image_version     = "v1.0.0-192-g020090f37"
  kratos_image_remote_name = "leancode.azurecr.io/kratos"
  kratos_image_local_name  = "${local.registry_address}/kratos"
  kratos_image_remote      = "${local.kratos_image_remote_name}:${local.kratos_image_version}"
  kratos_image_local       = "${local.kratos_image_local_name}:${local.kratos_image_version}"

  kratos_image_needs_retagging = startswith(local.kratos_image_remote_name, "docker.io/") ? 0 : 1
}

resource "docker_image" "kratos_remote" {
  count = local.kratos_image_needs_retagging

  name = local.kratos_image_remote
}

resource "docker_tag" "kratos" {
  count = local.kratos_image_needs_retagging

  source_image = docker_image.kratos_remote[0].name
  target_image = local.kratos_image_local
}

resource "docker_registry_image" "kratos" {
  count = local.kratos_image_needs_retagging

  name                 = docker_tag.kratos[0].target_image
  insecure_skip_verify = true

  depends_on = [null_resource.cluster_kubeconfig]
}

module "kratos" {
  source     = "git@github.com:leancodepl/terraform-kratos-module.git//kratos?ref=5f5d2a06cd9d8d314b1f7b75ea1fa0c323e7b5fd"
  depends_on = [kubernetes_namespace_v1.kratos, postgresql_grant.kratos, postgresql_grant.kratos_public]

  namespace    = kubernetes_namespace_v1.kratos.metadata[0].name
  project      = "exampleapp"
  ingress_host = "auth.local.lncd.pl"
  image        = local.kratos_image_needs_retagging != 0 ? docker_registry_image.kratos[0].name : local.kratos_image_remote
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
    api                 = "http://exampleapp-api-svc.exampleapp-dev.svc.cluster.local"
    domain              = "local.lncd.pl"
    oidc_config         = var.oidc_config
    authority_name      = "ExampleApp (dev)"
    web_hook_api_key    = "Passw12#"
    webauthn_origins    = var.webauthn_origins
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
          image = "docker.io/oryd/kratos-selfservice-ui-node:v1.0.0"
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
      host = "local.lncd.pl"
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
