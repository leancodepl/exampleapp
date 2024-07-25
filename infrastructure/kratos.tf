resource "random_password" "kratos_web_hook_api_key" {
  length  = 64
  special = false
}

module "kratos" {
  source     = "git::https://github.com/leancodepl/terraform-kratos-module.git//kratos?ref=v0.2.0"
  depends_on = [postgresql_grant.public["kratos"]]

  namespace    = data.kubernetes_namespace_v1.main.metadata[0].name
  labels       = merge(local.tags, { component = "kratos" })
  project      = local.project
  ingress_host = null
  image        = "leancodepublic.azurecr.io/kratos:v1.2.0-3-gdcc4a5c5"
  replicas     = 1

  resources = {
    requests = {
      cpu    = "100m"
      memory = "128Mi"
    }
    limits = {
      cpu    = "250m"
      memory = "256Mi"
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
    for f in fileset("${path.module}/kratos", "*") : f => file("${path.module}/kratos/${f}")
  }

  config_yaml = templatefile("${path.module}/kratos.yaml", {
    api                 = "http://exampleapp-examples-api.${data.kubernetes_namespace_v1.main.metadata[0].name}.svc.cluster.local"
    domain              = var.domain
    oidc_config         = var.oidc_config
    authority_name      = "ExampleApp"
    web_hook_api_key    = random_password.kratos_web_hook_api_key.result
    passkey_origins     = var.kratos_passkey_origins
    dev_allowed_origins = var.kratos_dev_allowed_origins
  })

  dsn = module.postgresql.roles["kratos"].libpg_uri_connection_string

  courier_smtp_connection_uri = "smtps://apikey:${var.kratos_sendgrid_api_key}@smtp.sendgrid.net:465"

  env = [
    {
      name = "AGENT_HOST_IP"
      value_from = {
        field_ref = {
          api_version = "v1"
          field_path  = "status.hostIP"
        }
      }
    },
    {
      name  = "TRACING_PROVIDERS_OTLP_SERVER_URL"
      value = "$(AGENT_HOST_IP):55681"
    }
  ]
}
