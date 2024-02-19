@echo off
setlocal

:: Generate a unique temporary folder name using the current date and time
set RAND_VAL=%RANDOM%
set TEMP_FOLDER=%TEMP%\MyTempFolder_%RAND_VAL%

:: Create the temporary folder
mkdir "%TEMP_FOLDER%"

:: Your commands that use the temporary folder go here
echo Temporary folder created at: %TEMP_FOLDER%

SET SOFG_MOD_INSTALL_DIR=%TEMP_FOLDER%\

set SOFG_ASSEMBLY_NAME=SoFG_UIMod%RAND_VAL%

dotnet build -p:AssemblyName=%SOFG_ASSEMBLY_NAME%

echo reload_mod %SOFG_MOD_INSTALL_DIR%%SOFG_ASSEMBLY_NAME%.dll|ncat 127.0.0.1 1331