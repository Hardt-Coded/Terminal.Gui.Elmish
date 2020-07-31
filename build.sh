#!/usr/bin/env bash

dotnet tool restore
FAKE_DETAILED_ERRORS=true dotnet fake build -t "$@"