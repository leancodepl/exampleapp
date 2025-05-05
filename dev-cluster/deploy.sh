#!/usr/bin/env zsh
set -e

cd "${0:A:h}"

if [[ $(uname) == "Darwin" ]]; then
  export K3D_FIX_DNS=0
fi

k3d cluster delete exampleapp || true
k3d registry delete k3d-exampleapp-registry.local.lncd.pl || true
rm *.tfstate* || true
docker rm exampleapp-certificates || true

# Docker provider will not be able to use the token
az acr login -n leancode && docker pull leancode.azurecr.io/traefik-proxy || true

# We depend on these charts
helm repo add traefik https://helm.traefik.io/traefik || true
helm repo update

# Some providers depend on each other and it looks like we cannot force terraform to respect that
# so some partial applications

terraform init
terraform apply -target data.local_file.kubeconfig -auto-approve
terraform apply -target helm_release.traefik -auto-approve
terraform apply -auto-approve
