locals {
  ingress = {
    hosts = ["api.${var.domain}", "auth.${var.domain}", var.domain]
    rules = [
      {
        rule     = "Host(`auth.${var.domain}`) && PathPrefix(`/.well-known/`) && !PathPrefix(`/.well-known/ory/`)"
        service  = "${local.project}-api"
        port     = 80
        priority = 100
      },
      {
        rule     = "Host(`${var.domain}`) && PathPrefix(`/api/`)"
        service  = "${local.project}-api"
        port     = 80
        priority = 50
      },
      {
        rule     = "Host(`${var.domain}`) && PathPrefix(`/leanpipe`)"
        service  = "${local.project}-api"
        port     = 80
        priority = 50
      },
      {
        rule     = "Host(`${var.domain}`) && PathPrefix(`/.well-known/`)"
        service  = "${local.project}-api"
        port     = 80
        priority = 50
      },
      {
        rule     = "Host(`auth.${var.domain}`)"
        service  = module.kratos.service_name
        port     = 80
        priority = 1
      },
      {
        rule     = "Host(`api.${var.domain}`)"
        service  = "${local.project}-api"
        port     = 80
        priority = 1
      },
      {
        rule     = "Host(`${var.domain}`)"
        service  = "${local.project}-kratos-ui-svc"
        port     = 80
        priority = 1
      },
    ]
  }
}

resource "kubernetes_manifest" "response_compression" {
  manifest = {
    "apiVersion" = "traefik.containo.us/v1alpha1"
    "kind"       = "Middleware"
    "metadata" = {
      "name"      = "${local.project}-compress-response"
      "namespace" = data.kubernetes_namespace_v1.main.metadata[0].name
      "labels"    = local.tags
    }
    "spec" = {
      "compress" = {}
    }
  }
}

resource "kubernetes_manifest" "ingress" {
  manifest = {
    "apiVersion" = "traefik.containo.us/v1alpha1"
    "kind"       = "IngressRoute"
    "metadata" = {
      "name"      = "${local.project}-ingress"
      "namespace" = data.kubernetes_namespace_v1.main.metadata[0].name
      "labels"    = local.tags
    }
    "spec" = {
      "entryPoints" = [
        "web",
        "websecure",
      ]
      "routes" = [
        for i in local.ingress.rules : {
          "kind"  = "Rule"
          "match" = i.rule
          "middlewares" = [
            {
              "name" = kubernetes_manifest.response_compression.manifest.metadata.name
            },
          ]
          "services" = [
            {
              "name" = i.service
              "port" = i.port
            },
          ]
          "priority" = i.priority
        }
      ]

      "tls" = {
        "certResolver" = "le"
      }
    }
  }
}

// https://github.com/traefik/traefik/issues/4655#issuecomment-878072714
resource "kubernetes_manifest" "ingress_external_dns_hack" {
  manifest = {
    "apiVersion" = "networking.k8s.io/v1"
    "kind"       = "Ingress"
    "metadata" = {
      "name"      = "${local.project}-ingress-external-dns"
      "namespace" = data.kubernetes_namespace_v1.main.metadata[0].name
      "labels"    = local.tags
    }
    "spec" = {
      "rules" = [
        for host in local.ingress.hosts :
        {
          "host" = host
        }
      ]
    }
  }
}
