module "app_config" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//app_config?ref=6e78fdc31d4503aa1a1f7838f80e9c35e2d182b6"

  key_vault_id                       = module.key_vault.vault_id
  key_vault_deploy_policy_depends_on = module.key_vault.deploy_policy

  key_vault_access_policy = {
    tenant_id = var.azure.tenant_id
    object_id = module.managed_identity_api.managed_identity.object_id
  }

  key_vault_secrets = {
    "Kratos--WebhookApiKey"         = random_password.kratos_web_hook_api_key.result
    "BlobStorage--ConnectionString" = module.storage.storage_connection_string

    "PostgreSQL--ConnectionString"           = module.postgresql.ad_roles[module.managed_identity_api.managed_identity.name].npg_connection_string
    "MassTransit--AzureServiceBus--Endpoint" = module.service_bus.service_bus_endpoint
  }

  k8s_namespace = data.kubernetes_namespace_v1.main.metadata[0].name
  k8s_config_maps = {
    "exampleapp-api-config" = {
      labels = merge(local.tags, { component = "api" })
      data = {
        "ASPNETCORE_ENVIRONMENT" = title(var.environment)

        "Logging__MinimumLevel"               = "Information"
        "Logging__EnableDetailedInternalLogs" = "true"

        "Azure__UseAzureWorkloadIdentity" = "true"

        "Kratos__PublicEndpoint" = module.kratos.internal_service_url.public
        "Kratos__AdminEndpoint"  = module.kratos.internal_service_url.admin
      }
    }
  }
  k8s_secrets = {
    "exampleapp-migrations-secret" = {
      labels = merge(local.tags, { component = "migrations" })
      data = {
        "Azure__UseAzureWorkloadIdentity" = "true"

        "PostgreSQL__ConnectionString" = module.postgresql.ad_roles[module.managed_identity_migrations.managed_identity.name].npg_connection_string
      }
    }
  }
}
