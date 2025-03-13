module "service_bus" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//azure_service_bus?ref=v0.4.3"

  resource_group_name = data.azurerm_resource_group.main.name
  service_bus_name    = local.azure_resource_name

  data_owners_object_ids = { examples_api = module.managed_identity_examples_api.managed_identity.object_id }

  tags = var.tags
}
