:: create nuget package
dotnet pack CoreCmd\CoreCmd.csproj -c Release -o E:\rp\local-nuget-packages

:: create and install global "core" command
dotnet pack Core -c Release
dotnet tool update -g core --add-source ./core/bin/NuPkg
