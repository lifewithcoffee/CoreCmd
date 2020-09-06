@echo off

::dotnet run %*

:: =========================================================================
SET DEBUG_PATH=%CD%\bin\Debug\netcoreapp3.1
dotnet %DEBUG_PATH%\CoreProject.dll %*

:: =========================================================================
:: or set the debug path to system path, then use the following statement to
:: replace the above one:
:: 
:: dotnet %~dp0DependentConsoleApp.dll %*
@echo on