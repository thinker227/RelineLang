# Script to pack and install the current version of the compiler CLI
Set-Location ./Reline.CommandLine
dotnet pack --no-restore
dotnet tool install --global --version 1.0.0-dev --add-source ./nupkg Reline.CommandLine
