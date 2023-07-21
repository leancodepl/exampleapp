module "managed_identity_api" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//managed_identity?ref=6e78fdc31d4503aa1a1f7838f80e9c35e2d182b6"

  azure_resource_group  = data.azurerm_resource_group.main.name
  managed_identity_name = "${local.project}-${var.environment}-api"

  kubernetes = {
    azure_resource_group    = var.azure.aks.resource_group_name
    kubernetes_service_name = var.azure.aks.name
    namespace               = data.kubernetes_namespace_v1.main.metadata[0].name
  }

  azure_role_assignments = []

  depends_on = [data.azurerm_resource_group.main]
}

module "managed_identity_migrations" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//managed_identity?ref=6e78fdc31d4503aa1a1f7838f80e9c35e2d182b6"

  azure_resource_group  = data.azurerm_resource_group.main.name
  managed_identity_name = "${local.project}-${var.environment}-migrations"

  kubernetes = {
    azure_resource_group    = var.azure.aks.resource_group_name
    kubernetes_service_name = var.azure.aks.name
    namespace               = data.kubernetes_namespace_v1.main.metadata[0].name
  }

  azure_role_assignments = []

  depends_on = [data.azurerm_resource_group.main]
}
