#!/usr/bin/env zsh
set -eo pipefail

SCRIPT_PATH="${0:A:h}"
PROJ_TEMP=$(mktemp -d)

dotnet new install "$SCRIPT_PATH/.." || true

pushd "$PROJ_TEMP"
dotnet new lncdproj --project-name TestProj --context TestProj --allow-scripts Yes || true
(cd backend && dotnet build) || true
popd

rm -rf "$PROJ_TEMP"
