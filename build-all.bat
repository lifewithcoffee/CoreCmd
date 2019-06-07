:: create console app dlls
dotnet publish CoreCmd\CoreCmd.csproj -c Release -o bin\Publish

:: create nuget package
dotnet pack CoreCmd\CoreCmd.csproj -c Release -o E:\rp\local-nuget-packages

:: update ExperimentalConsoleApp nuget dependency
dotnet add ExperimentalConsoleApp\ExperimentalConsoleApp.csproj package corecmd
dotnet restore ExperimentalConsoleApp
dotnet build ExperimentalConsoleApp