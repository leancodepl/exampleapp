#!/usr/bin/env bash

/etc/init.d/ssh start
source /app/config/config.sh
exec -a ExampleApp.Api dotnet /app/bin/ExampleApp.Api.dll
