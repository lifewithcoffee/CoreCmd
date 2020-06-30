SET PACK_OUTPUT_DIR=E:\rp\local-nuget-packages

:: create corecmd lib package
dotnet pack CoreCmd\CoreCmd.csproj -c Release -o %PACK_OUTPUT_DIR%

:: create corecmd.cli global command package
dotnet pack Corecmd.cli -c Release -o %PACK_OUTPUT_DIR%

:: install CoreCmd.Cli global command (aka. the "core" command):
dotnet tool update -g corecmd.cli --add-source %PACK_OUTPUT_DIR%
