variable "traefik_self_signed" {
  type    = bool
  default = true
}

variable "metabase" {
  type    = bool
  default = false
}

variable "rabbit" {
  type    = bool
  default = true
}

variable "sendgrid_api_key" {
  type = string
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
  default = null
}

variable "webauthn_origins" {
  type    = list(string)
  default = []
}
