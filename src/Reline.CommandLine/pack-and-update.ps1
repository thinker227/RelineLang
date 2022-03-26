# Script to pack and update the current version of the compiler CLI
dotnet pack --no-restore
dotnet tool update --global --version 1.0.0-dev --add-source ./nupkg Reline.CommandLine
