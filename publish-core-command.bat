SET PACK_OUTPUT_DIR=E:\rp\local-nuget-packages

:: create nuget package
dotnet pack CoreCmd\CoreCmd.csproj -c Release -o %PACK_OUTPUT_DIR%

:: create and install global "core" command
dotnet pack Corecmd.cli -c Release -o %PACK_OUTPUT_DIR%

:: install CoreCmd.Cli locally by:
dotnet tool update -g corecmd.cli --add-source %PACK_OUTPUT_DIR%
