module "key_vault" {
  source = "git@github.com:leancodepl/terraform-common-modules.git//key_vault?ref=6e78fdc31d4503aa1a1f7838f80e9c35e2d182b6"

  resource_group_name = data.azurerm_resource_group.main.name
  name                = local.azure_resource_name

  network_acls = {
    allow_all  = var.azure.network_allow_all
    subnet_ids = [var.azure.aks.subnet_id]
    ip_ranges  = ["${var.office_ip}/32"]
  }

  owner_access_policy = {
    tenant_id = var.azure.tenant_id
    object_id = data.azuread_group.project.object_id
  }

  tags = local.tags
}

resource "azurerm_key_vault_key" "sops" {
  depends_on   = [module.key_vault.deploy_policy]
  key_vault_id = module.key_vault.vault_id
  name         = "sops"
  key_opts     = ["encrypt", "decrypt"]
  key_size     = 2048
  key_type     = "RSA"
}
