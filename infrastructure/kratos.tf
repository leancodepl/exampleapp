resource "random_password" "kratos_web_hook_api_key" {
  length  = 64
  special = false
}

module "kratos" {
  source     = "git@github.com:leancodepl/terraform-kratos-module.git//kratos?ref=v0.1.0"
  depends_on = [postgresql_grant.public["kratos"]]

  namespace    = data.kubernetes_namespace_v1.main.metadata[0].name
  labels       = merge(local.tags, { component = "kratos" })
  project      = local.project
  ingress_host = null
  image        = "docker.io/oryd/kratos:v1.0.0"
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
    api                 = "http://exampleapp-api.${data.kubernetes_namespace_v1.main.metadata[0].name}.svc.cluster.local"
    domain              = var.domain
    oidc_config         = var.oidc_config
    totp_issuer         = "ExampleApp"
    web_hook_api_key    = random_password.kratos_web_hook_api_key.result
    dev_allowed_origins = var.kratos_dev_allowed_origins
  })

  dsn = module.postgresql.roles["kratos"].libpg_uri_connection_string

  courier_smtp_connection_uri = "smtps://apikey:${var.kratos_sendgrid_api_key}@smtp.sendgrid.net:465"
}
