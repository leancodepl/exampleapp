#!/usr/bin/env bash

source dev/config/config.sh

dotnet ef migrations bundle -o dev/out/migrations --project src/Examples/ExampleApp.Examples.Api --force
