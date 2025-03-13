module "storage" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//azure_blob_storage?ref=v0.4.3"

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
  tags            = var.tags
}

module "storage_assets" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//azure_blob_storage_assets?ref=v0.4.3"

  storage_account_name = module.storage.storage_account_name
  container_name       = module.storage.storage_containers["public"].name

  base_dir = "${path.module}/storage-assets"
}

resource "azurerm_storage_table" "audit_logs" {
  name                 = "auditlogs"
  storage_account_name = module.storage.storage_account_name
}

//#if Example
resource "azurerm_storage_container" "service_providers" {
  name = "service-providers"

  storage_account_id    = module.storage.storage_account_id
  container_access_type = "blob"
}

resource "azurerm_storage_management_policy" "rules" {
  storage_account_id = module.storage.storage_account_id

  rule {
    name    = "delete_unused_blobs"
    enabled = true
    filters {
      prefix_match = ["${azurerm_storage_container.service_providers.name}/"]
      blob_types   = ["blockBlob"]
      match_blob_index_tag {
        name      = "ToDelete"
        operation = "=="
        value     = "1"
      }
    }
    actions {
      base_blob {
        delete_after_days_since_modification_greater_than = 1
      }
    }
  }
}
//#endif
