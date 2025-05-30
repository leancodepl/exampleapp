locals {
  databases = {
    "examples" = {
      charset   = "UTF8"
      collation = "en_US.utf8"

      roles = {}

      ad_roles = {
        app_role        = module.managed_identity_examples_api.managed_identity.name
        migrations_role = module.managed_identity_examples_migrations.managed_identity.name
      }
    }
    "kratos" = {
      charset   = "UTF8"
      collation = "en_US.utf8"

      roles = {
        "kratos" = {
          privileges = ["CREATE"]
        }
      }
    }
    "metabase" = {
      charset   = "UTF8"
      collation = "en_US.utf8"

      roles = {
        "metabase" = {
          privileges = ["CREATE"]
        }
      }
    }
  }
}

module "postgresql" {
  source = "git::https://github.com/leancodepl/terraform-common-modules.git//postgresql?ref=v0.4.3"

  resource_group = {
    name     = data.azurerm_resource_group.main.name
    location = data.azurerm_resource_group.main.location
  }

  server = {
    name                = local.azure_resource_name
    storage_mb          = "32768"
    sku_name            = "B_Standard_B1ms"
    version             = "15"
    administrator_login = "azureadmin"
  }

  ad_admin = {
    tenant_id      = var.azure.tenant_id
    object_id      = data.azuread_group.project.object_id
    principal_name = data.azuread_group.project.display_name
    principal_type = "Group"
  }

  databases = local.databases

  firewall = {
    allow_all = var.azure.network_allow_all
    ip_rules = {
      k8s    = { start_ip = var.kubernetes.egress_public_ip, end_ip = var.kubernetes.egress_public_ip }
      office = { start_ip = var.office_ip, end_ip = var.office_ip }
    }
  }

  tags = var.tags
}

resource "postgresql_grant" "public" {
  depends_on  = [module.postgresql]
  for_each    = local.databases
  database    = each.key
  object_type = "schema"
  schema      = "public"
  role        = "public"
  privileges  = ["USAGE", "CREATE"]
}

output "postgresql_server_fqdn" {
  value     = module.postgresql.server_fqdn
  sensitive = false
}

output "postgresql_administrator_login" {
  value     = module.postgresql.administrator_login
  sensitive = false
}

output "postgresql_administrator_password" {
  value     = module.postgresql.administrator_password
  sensitive = true
}

output "postgresql_ad_roles_config" {
  value     = module.postgresql.ad_setup_config
  sensitive = true
}

output "postgresql_ad_roles_script" {
  value     = module.postgresql.ad_setup_script
  sensitive = false
}
