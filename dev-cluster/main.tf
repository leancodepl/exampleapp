terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "3.0.2"
    }
    helm = {
      source  = "hashicorp/helm"
      version = "2.10.1"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "2.21.1"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = "1.19.0"
    }
  }
}


provider "kubernetes" {
  host                   = local.credentials.host
  client_certificate     = local.credentials.client_certificate
  client_key             = local.credentials.client_key
  cluster_ca_certificate = local.credentials.cluster_ca_certificate

  experiments {
    manifest_resource = true
  }
}

provider "helm" {
  kubernetes {
    host                   = local.credentials.host
    client_certificate     = local.credentials.client_certificate
    client_key             = local.credentials.client_key
    cluster_ca_certificate = local.credentials.cluster_ca_certificate
  }
}
