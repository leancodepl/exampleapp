module "service_bus" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//azure_service_bus?ref=d76e385f387dfc851623d8aac92a751874a998bf"

  resource_group_name = data.azurerm_resource_group.main.name
  service_bus_name    = local.azure_resource_name

  data_owners_object_ids = { api = module.managed_identity_api.managed_identity.object_id }

  tags = local.tags
}
