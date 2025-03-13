module "managed_identity_examples_api" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//managed_identity?ref=v0.4.3"

  azure_resource_group  = data.azurerm_resource_group.main.name
  managed_identity_name = "${local.azure_resource_name}-examples-api"
  service_account_name  = "${local.project}-examples-api"

  kubernetes = {
    azure_resource_group    = var.azure.aks.resource_group_name
    kubernetes_service_name = var.azure.aks.name
    namespace               = data.kubernetes_namespace_v1.main.metadata[0].name
  }

  azure_role_assignments = []

  tags = var.tags

  depends_on = [data.azurerm_resource_group.main]
}

module "managed_identity_examples_migrations" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//managed_identity?ref=v0.4.3"

  azure_resource_group  = data.azurerm_resource_group.main.name
  managed_identity_name = "${local.azure_resource_name}-examples-migrations"
  service_account_name  = "${local.project}-examples-migrations"

  kubernetes = {
    azure_resource_group    = var.azure.aks.resource_group_name
    kubernetes_service_name = var.azure.aks.name
    namespace               = data.kubernetes_namespace_v1.main.metadata[0].name
  }

  azure_role_assignments = []

  tags = var.tags

  depends_on = [data.azurerm_resource_group.main]
}
