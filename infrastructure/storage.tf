module "storage" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//azure_blob_storage?ref=d76e385f387dfc851623d8aac92a751874a998bf"

  resource_group_name  = data.azurerm_resource_group.main.name
  storage_account_name = replace(local.azure_resource_name, "-", "")

  data_owners_object_ids = { api = module.managed_identity_api.managed_identity.object_id }

  blob_containers = {
    "public" = {
      access_type = "blob"
    }
  }

  blob_cors_rules = []
  tags            = local.tags
}

module "storage_assets" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//azure_blob_storage_assets?ref=d76e385f387dfc851623d8aac92a751874a998bf"

  storage_account_name = module.storage.storage_account_name
  container_name       = module.storage.storage_containers["public"].name

  base_dir = "./storage-assets"
}
