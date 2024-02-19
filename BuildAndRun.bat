@echo off
taskkill /F /IM ShadowsOfForbiddenGods.exe
SET SOFG_MOD_INSTALL_DIR=D:\Personal\Steam\steamapps\common\Shadows of Forbidden Gods\data\optionalData\UI_improvements\
dotnet build
start "" /B /D "D:\Personal\Steam\steamapps\common\Shadows of Forbidden Gods" "D:\Personal\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods.exe"