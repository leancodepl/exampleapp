module "app_config" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//app_config?ref=v0.4.3"

  key_vault_id                       = module.key_vault.vault_id
  key_vault_deploy_policy_depends_on = module.key_vault.deploy_policy

  key_vault_access_policy = {
    tenant_id = var.azure.tenant_id
    object_id = module.managed_identity_examples_api.managed_identity.object_id
  }

  key_vault_secrets = merge(var.key_vault_secrets, {
    "Kratos--WebhookApiKey"         = random_password.kratos_web_hook_api_key.result
    "BlobStorage--ConnectionString" = module.storage.storage_connection_string

    "PostgreSQL--ConnectionString"           = module.postgresql.ad_roles[module.managed_identity_examples_api.managed_identity.name].npg_connection_string
    "MassTransit--AzureServiceBus--Endpoint" = module.service_bus.service_bus_endpoint
  })

  k8s_namespace = data.kubernetes_namespace_v1.main.metadata[0].name

  k8s_config_maps = {
    "exampleapp-wellknown" = {
      labels = var.tags
      data   = var.well_known
    }

    "exampleapp-web-config" = {
      labels = merge(var.tags, { component = "web" })
      data = {
        "NX_API_BASE"  = "https://api.${var.domain}",
        "NX_AUTH_BASE" = "https://auth.${var.domain}",
      }
    }
  }

  k8s_secrets = {
    "exampleapp-examples-api-secret" = {
      labels = merge(var.tags, { component = "api" })
      data = merge(local.backend_cors_allowed_origins, {
        "ASPNETCORE_ENVIRONMENT" = title(var.environment)

        "Logging__MinimumLevel"               = "Information"
        "Logging__EnableDetailedInternalLogs" = "true"

        "Azure__UseAzureWorkloadIdentity" = "true"
        "KeyVault__VaultUrl"              = module.key_vault.vault_url

        "Kratos__PublicEndpoint" = module.kratos.internal_service_url.public
        "Kratos__AdminEndpoint"  = module.kratos.internal_service_url.admin

        "AuditLogs__ContainerName" = module.storage.storage_containers["audit-logs"].name
        "AuditLogs__TableName"     = azurerm_storage_table.audit_logs.name

        "Metabase__Url"       = "https://${kubernetes_ingress_v1.metabase_ingress.spec[0].rule[0].host}"
        "Metabase__SecretKey" = random_password.metabase_embedding_key.result
        //#if Example
        "Metabase__AssignmentEmployerEmbedQuestion" = 1
        //#endif
      })
    }
    "exampleapp-examples-migrations-secret" = {
      labels = merge(var.tags, { component = "migrations" })
      data = {
        "Azure__UseAzureWorkloadIdentity" = "true"

        "PostgreSQL__ConnectionString" = module.postgresql.ad_roles[module.managed_identity_examples_migrations.managed_identity.name].npg_connection_string
      }
    }
  }
}

locals {
  backend_cors_allowed_origins = {
    for idx, element in concat(["https://${var.domain}"], var.backend_dev_allowed_origins) : "CORS__AllowedOrigins__${idx}" => element
  }
}
