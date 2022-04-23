# Script to pack and install the current version of the compiler CLI
dotnet build --configuration Release
dotnet pack --no-build
dotnet tool install --global --version 1.0.0-dev --add-source ./nupkg Reline.CommandLine
