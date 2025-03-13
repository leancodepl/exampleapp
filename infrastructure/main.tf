terraform {
  required_version = ">= 1.11.0"

  required_providers {
    azuread = {
      source  = "hashicorp/azuread"
      version = ">= 3.1"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 4.22"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = ">= 2.36"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = ">= 1.25"
    }
    random = {
      source  = "hashicorp/random"
      version = ">= 3.7"
    }
  }
}

data "azuread_group" "project" {
  object_id = var.azure.ad_project_group_id
}

data "azurerm_resource_group" "main" {
  name = var.azure.resource_group_name
}

data "kubernetes_namespace_v1" "main" {
  metadata {
    name = var.kubernetes.namespace_name
  }
}

locals {
  project = "exampleapp"

  azure_resource_name = join("-", [
    //#if Example
    "lncd", # prefix to avoid resource FQDN collisions
    //#endif
    local.project,
    var.environment
  ])

  tags = {
    project     = local.project
    environment = var.environment
  }
}
