module "kratos" {
  source     = "git@github.com:leancodepl/terraform-kratos-module.git//kratos?ref=5f5d2a06cd9d8d314b1f7b75ea1fa0c323e7b5fd"
  depends_on = [kubernetes_namespace_v1.kratos, postgresql_grant.kratos, postgresql_grant.kratos_public]

  namespace    = kubernetes_namespace_v1.kratos.metadata[0].name
  project      = "exampleapp"
  ingress_host = "auth.local.lncd.pl"
  image        = "docker.io/oryd/kratos:v1.0.0"
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
    api              = "http://exampleapp-api-svc.exampleapp-dev.svc.cluster.local"
    domain           = "local.lncd.pl"
    totp_issuer      = "ExampleApp (dev)"
    web_hook_api_key = "Passw12#"
  })

  dsn = "postgresql://${urlencode(postgresql_role.kratos.name)}:${urlencode(postgresql_role.kratos.password)}@${kubernetes_service_v1.postgresql_service.metadata[0].name}.${kubernetes_service_v1.postgresql_service.metadata[0].namespace}.svc.cluster.local/${postgresql_database.kratos.name}?sslmode=disable"

  courier_smtp_connection_uri = "smtps://apikey:${var.sendgrid_api_key}@smtp.sendgrid.net:465"
}
