module "service_bus" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//azure_service_bus?ref=6e78fdc31d4503aa1a1f7838f80e9c35e2d182b6"

  resource_group_name = data.azurerm_resource_group.main.name
  service_bus_name    = local.azure_resource_name

  data_owners_object_ids = { api = module.managed_identity_api.managed_identity.object_id }

  tags = local.tags
}
