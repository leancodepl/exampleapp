locals {
  k8s_shared_namespace = kubernetes_namespace_v1.shared.metadata[0].name
  k8s_main_namespace   = kubernetes_namespace_v1.main.metadata[0].name
  k8s_blob_namespace   = kubernetes_namespace_v1.blob.metadata[0].name
  k8s_kratos_namespace = kubernetes_namespace_v1.kratos.metadata[0].name
}

resource "kubernetes_namespace_v1" "shared" {
  metadata {
    name = "shared"
  }
}

resource "kubernetes_namespace_v1" "main" {
  metadata {
    name = "exampleapp-dev"
  }
}

resource "kubernetes_namespace_v1" "blob" {
  metadata {
    name = "blob"
  }
}

resource "kubernetes_namespace_v1" "kratos" {
  metadata {
    name = "kratos"
  }
}

resource "kubernetes_config_map_v1" "well_known" {
  metadata {
    namespace = kubernetes_namespace_v1.main.metadata[0].name
    name      = "exampleapp-wellknown"
  }
  data = {
    for f in fileset("./.well-known", "*") : f => file("./.well-known/${f}")
  }
}
