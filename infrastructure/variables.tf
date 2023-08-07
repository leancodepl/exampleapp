variable "azure" {
  type = object({
    ad_project_group_id = string
    tenant_id           = string
    subscription_id     = string
    resource_group_name = string
    network_allow_all   = bool

    aks = object({
      resource_group_name = string
      name                = string
      subnet_id           = string
    })
  })
}

variable "domain" {
  type = string
}

variable "environment" {
  type = string
}

variable "key_vault_secrets" {
  type      = map(string)
  sensitive = true
}

variable "kratos_sendgrid_api_key" {
  type      = string
  sensitive = true
}

variable "kubernetes" {
  type = object({
    egress_public_ip = string
    namespace_name   = string
  })
}

variable "oidc_config" {
  type = object({
    apple = object({
      client_id      = string
      team_id        = string
      private_key_id = string
      private_key    = string
    })
    google = object({
      client_id     = string
      client_secret = string
    })
    facebook = object({
      client_id     = string
      client_secret = string
    })
  })
}

variable "office_ip" {
  type = string
}

variable "well_known" {
  type    = map(string)
}
