terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "3.0.2"
    }
    helm = {
      source  = "hashicorp/helm"
      version = "2.15.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "2.32.0"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = "1.23.0"
    }
  }
}


provider "kubernetes" {
  host                   = local.credentials.host
  client_certificate     = local.credentials.client_certificate
  client_key             = local.credentials.client_key
  cluster_ca_certificate = local.credentials.cluster_ca_certificate
}

provider "helm" {
  kubernetes {
    host                   = local.credentials.host
    client_certificate     = local.credentials.client_certificate
    client_key             = local.credentials.client_key
    cluster_ca_certificate = local.credentials.cluster_ca_certificate
  }
}

provider "docker" {
  registry_auth {
    address       = "http://${local.registry_address}"
    auth_disabled = true
  }
}
