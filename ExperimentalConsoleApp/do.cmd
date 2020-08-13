@echo off

::dotnet run %*

:: =========================================================================
SET DEBUG_PATH=%CD%\bin\Debug\netcoreapp3.1
dotnet %DEBUG_PATH%\ExperimentalConsoleApp.dll %*

:: =========================================================================
:: or set the debug path to system path, then use the following statement to
:: replace the above one:
:: 
:: dotnet %~dp0ExperimentalConsoleApp.dll %*
@echo on