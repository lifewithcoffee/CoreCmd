dotnet publish CoreCmd\CoreCmd.csproj -c Release -o e:\rp\git\CoreCmd\CoreCmd\bin\Publish
dotnet pack CoreCmd\CoreCmd.csproj -c Release -o E:\rp\local-nuget-packages
dotnet add ExperimentalConsoleApp\ExperimentalConsoleApp.csproj package corecmd
dotnet restore ExperimentalConsoleApp
dotnet build ExperimentalConsoleApp