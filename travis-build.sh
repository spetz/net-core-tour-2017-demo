#!/bin/bash

PROJECTS=(src/Depot.Api tests/Depot.Api.Tests)
for PROJECT in ${PROJECTS[*]}
do
  dotnet restore $PROJECT --no-cache
  dotnet build $PROJECT
done


