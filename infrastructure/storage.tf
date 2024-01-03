module "storage" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//azure_blob_storage?ref=v0.1.0"

  resource_group_name  = data.azurerm_resource_group.main.name
  storage_account_name = replace(local.azure_resource_name, "-", "")

  data_owners_object_ids = { examples_api = module.managed_identity_examples_api.managed_identity.object_id }

  blob_containers = {
    "public" = {
      access_type = "blob"
    }
    "audit-logs" = {
      access_type = "private"
    }
  }

  blob_cors_rules = []
  tags            = local.tags
}

module "storage_assets" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//azure_blob_storage_assets?ref=v0.1.0"

  storage_account_name = module.storage.storage_account_name
  container_name       = module.storage.storage_containers["public"].name

  base_dir = "./storage-assets"
}

resource "azurerm_storage_table" "audit_logs" {
  name                 = "auditlogs"
  storage_account_name = module.storage.storage_account_name
}
