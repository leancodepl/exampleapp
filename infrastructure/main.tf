terraform {
  required_version = ">= 1.5.0"

  required_providers {
    azuread = {
      source  = "hashicorp/azuread"
      version = ">= 2.40"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.65"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = ">= 2.20"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = ">= 1.20"
    }
    random = {
      source  = "hashicorp/random"
      version = ">= 3.5"
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
  project = "lncdexampleapp" # prefix to avoid resource FQDN collisions

  tags = {
    project     = local.project
    environment = var.environment
  }
}
