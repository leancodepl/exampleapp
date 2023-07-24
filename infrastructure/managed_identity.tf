module "managed_identity_api" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//managed_identity?ref=d76e385f387dfc851623d8aac92a751874a998bf"

  azure_resource_group  = data.azurerm_resource_group.main.name
  managed_identity_name = "${local.azure_resource_name}-api"
  service_account_name  = "${local.project}-api"

  kubernetes = {
    azure_resource_group    = var.azure.aks.resource_group_name
    kubernetes_service_name = var.azure.aks.name
    namespace               = data.kubernetes_namespace_v1.main.metadata[0].name
  }

  azure_role_assignments = []

  depends_on = [data.azurerm_resource_group.main]
}

module "managed_identity_migrations" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//managed_identity?ref=d76e385f387dfc851623d8aac92a751874a998bf"

  azure_resource_group  = data.azurerm_resource_group.main.name
  managed_identity_name = "${local.azure_resource_name}-migrations"
  service_account_name  = "${local.project}-migrations"

  kubernetes = {
    azure_resource_group    = var.azure.aks.resource_group_name
    kubernetes_service_name = var.azure.aks.name
    namespace               = data.kubernetes_namespace_v1.main.metadata[0].name
  }

  azure_role_assignments = []

  depends_on = [data.azurerm_resource_group.main]
}
