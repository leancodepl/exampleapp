#!/usr/bin/env bash

source ~/config/config.sh
rm ~/bin/appsettings.local.json
exec -a ExampleApp.Examples dotnet ~/bin/ExampleApp.Examples.dll
