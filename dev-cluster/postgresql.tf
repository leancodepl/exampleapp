provider "postgresql" {
  host     = "localhost"
  database = "postgres"
  username = "postgres"
  password = "Passw12#"
  sslmode  = "disable"

  connect_timeout = 60
}

resource "kubernetes_deployment_v1" "postgresql_deployment" {
  metadata {
    name      = "postgresql"
    namespace = local.k8s_shared_namespace
    labels = {
      app = "postgresql"
    }
  }
  spec {
    selector {
      match_labels = {
        app = "postgresql"
      }
    }
    template {
      metadata {
        labels = {
          app = "postgresql"
        }
      }
      spec {
        container {
          env {
            name  = "POSTGRES_PASSWORD"
            value = "Passw12#"
          }
          image = "postgres:17"
          name  = "postgresql"
          volume_mount {
            mount_path = "/var/lib/postgresql/data"
            name       = "data"
          }
        }
        volume {
          name = "data"
          persistent_volume_claim {
            claim_name = kubernetes_manifest.postgresql_pvc.manifest.metadata.name
          }
        }
      }
    }
  }
}

resource "kubernetes_manifest" "postgresql_pvc" {
  manifest = {
    "apiVersion" = "v1"
    "kind"       = "PersistentVolumeClaim"
    "metadata" = {
      "name"      = "postgresql-pvc"
      "namespace" = local.k8s_shared_namespace
      "labels" = {
        "app" = "postgresql"
      }
    }
    "spec" = {
      "accessModes" = [
        "ReadWriteOnce",
      ]
      "resources" = {
        "requests" = {
          "storage" = "10Gi"
        }
      }
    }
  }
}

resource "kubernetes_service_v1" "postgresql_service" {
  metadata {
    name      = "postgresql-svc"
    namespace = local.k8s_shared_namespace
    labels = {
      app = "postgresql"
    }
  }
  spec {
    port {
      port        = 5432
      target_port = 5432
    }
    selector = {
      app = "postgresql"
    }
    type = "LoadBalancer"
  }
}

resource "time_sleep" "postgres_setup_delay" {
  create_duration = "30s"

  depends_on = [
    kubernetes_deployment_v1.postgresql_deployment,
    kubernetes_service_v1.postgresql_service,
  ]
}

resource "postgresql_database" "examples" {
  name       = "examples"
  lc_collate = "en_US.utf8"

  depends_on = [
    time_sleep.postgres_setup_delay
  ]
}

resource "postgresql_role" "examples" {
  name     = "examples"
  login    = true
  password = "Passw12#"

  depends_on = [
    time_sleep.postgres_setup_delay
  ]
}

resource "postgresql_grant" "examples" {
  database    = postgresql_database.examples.name
  role        = postgresql_role.examples.name
  object_type = "database"
  privileges  = ["CREATE"]
}

resource "postgresql_grant" "examples_public" {
  database    = postgresql_database.examples.name
  role        = "public"
  object_type = "schema"
  schema      = "public"
  privileges  = ["USAGE", "CREATE"]
}

resource "postgresql_database" "kratos" {
  name       = "kratos"
  lc_collate = "en_US.utf8"

  depends_on = [
    time_sleep.postgres_setup_delay
  ]
}

resource "postgresql_role" "kratos" {
  name     = "kratos"
  login    = true
  password = "Passw12#"

  depends_on = [
    time_sleep.postgres_setup_delay
  ]
}

resource "postgresql_grant" "kratos" {
  database    = postgresql_database.kratos.name
  role        = postgresql_role.kratos.name
  object_type = "database"
  privileges  = ["CREATE"]
}

resource "postgresql_grant" "kratos_public" {
  database    = postgresql_database.kratos.name
  role        = "public"
  object_type = "schema"
  schema      = "public"
  privileges  = ["USAGE", "CREATE"]
}

resource "postgresql_database" "metabase" {
  name       = "metabase"
  lc_collate = "en_US.utf8"

  depends_on = [
    time_sleep.postgres_setup_delay
  ]
}

resource "postgresql_role" "metabase" {
  name     = "metabase"
  login    = true
  password = "Passw12#"

  depends_on = [
    time_sleep.postgres_setup_delay
  ]
}

resource "postgresql_grant" "metabase" {
  database    = postgresql_database.metabase.name
  role        = postgresql_role.metabase.name
  object_type = "database"
  privileges  = ["CREATE"]
}

resource "postgresql_grant" "metabase_public" {
  database    = postgresql_database.metabase.name
  role        = "public"
  object_type = "schema"
  schema      = "public"
  privileges  = ["USAGE", "CREATE"]
}
