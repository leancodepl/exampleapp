#!/usr/bin/env bash

export Logging__EnableDetailedInternalLogs=true
export Logging__MinimumLevel=Verbose
export Logging__SeqEndpoint='http://seq-svc.shared.svc.cluster.local'

export Kratos__PublicEndpoint='http://exampleapp-kratos-svc.kratos.svc.cluster.local'
export Kratos__AdminEndpoint='http://exampleapp-kratos-svc.kratos.svc.cluster.local:4434'
export Kratos__WebhookApiKey='Passw12#'

export PostgreSQL__ConnectionString='Host=postgresql-svc.shared.svc.cluster.local;Database=examples;Username=examples;Password=Passw12#'
export BlobStorage__ConnectionString='DefaultEndpointsProtocol=http;AccountName=azurite;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://azurite.shared.svc.cluster.local:80/;TableEndpoint=http://azurite.shared.svc.cluster.local:82/;'

export MassTransit__RabbitMq__Url='rabbitmq://user:user@rabbit-rabbitmq.shared.svc.cluster.local/'

export Metabase__Url="https://metabase.local.lncd.pl"
export Metabase__AssignmentEmployerEmbedQuestion=1
export Metabase__SecretKey='embedding_secret_key_that_needs_to_have_256_bits'

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

export ConfigCat__FlagOverridesJsonObject="$(< "${DIR}/ConfigCatFlagOverrides.json")"

export CORS__AllowedOrigins__0="https://local.lncd.pl"

export AuditLogs__ContainerName='audit'
export AuditLogs__TableName='audit'

if [[ -f "$DIR/secrets.sh" ]]
then
    source "$DIR/secrets.sh"
fi
