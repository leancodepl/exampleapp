resource "azurerm_storage_account" "audit_logs_storage" {
  location            = data.azurerm_resource_group.main.location
  resource_group_name = data.azurerm_resource_group.main.name
  name                = replace("${local.azure_resource_name}audit", "-", "")

  account_kind             = "StorageV2"
  account_replication_type = "LRS"
  account_tier             = "Standard"
  access_tier              = "Hot"

  allow_nested_items_to_be_public = false
  enable_https_traffic_only       = true

  blob_properties {
    last_access_time_enabled = true
  }

  min_tls_version = "TLS1_2"
}

resource "azurerm_storage_container" "audit_logs" {
  name                 = "audit-logs"
  storage_account_name = azurerm_storage_account.audit_logs_storage.name
}

resource "azurerm_storage_table" "audit_logs" {
  name                 = "auditlogs"
  storage_account_name = azurerm_storage_account.audit_logs_storage.name
}

resource "azurerm_storage_management_policy" "decrease_access_tier" {
  storage_account_id = azurerm_storage_account.audit_logs_storage.id

  rule {
    name    = "archive_and_delete_old_log_entries"
    enabled = true
    filters {
      prefix_match = ["${azurerm_storage_container.audit_logs.name}/"]
      blob_types   = ["appendBlob"]
    }
    actions {
      base_blob {
        tier_to_cool_after_days_since_modification_greater_than    = 28
        tier_to_archive_after_days_since_modification_greater_than = 84
        delete_after_days_since_modification_greater_than          = 365
      }
    }
  }
}
