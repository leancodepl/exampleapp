resource "random_password" "kratos_web_hook_api_key" {
  length  = 64
  special = false
}

module "kratos" {
  source     = "git@github.com:leancodepl/terraform-kratos-module.git//kratos?ref=5f5d2a06cd9d8d314b1f7b75ea1fa0c323e7b5fd"
  depends_on = [postgresql_grant.public["kratos"]]

  namespace    = data.kubernetes_namespace_v1.main.metadata[0].name
  labels       = merge(local.tags, { component = "kratos" })
  project      = local.project
  ingress_host = "auth.${var.domain}"
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
    api              = "http://exampleapp-api-svc.${data.kubernetes_namespace_v1.main.metadata[0].name}.svc.cluster.local"
    domain           = var.domain
    totp_issuer      = "ExampleApp"
    web_hook_api_key = random_password.kratos_web_hook_api_key.result
  })

  dsn = module.postgresql.roles["kratos"].libpg_uri_connection_string

  courier_smtp_connection_uri = "smtps://apikey:${var.kratos_sendgrid_api_key}@smtp.sendgrid.net:465"
}