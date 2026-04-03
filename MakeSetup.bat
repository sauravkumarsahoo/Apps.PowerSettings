@echo off
setlocal EnableExtensions

pushd "%~dp0" || goto :error

if not exist "VERSION" (
	echo ERROR: VERSION file not found.
	goto :error
)

set "VSDEVCMD="
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
	for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do set "VSINSTALLDIR=%%i"
	if defined VSINSTALLDIR if exist "%VSINSTALLDIR%\Common7\Tools\VsDevCmd.bat" set "VSDEVCMD=%VSINSTALLDIR%\Common7\Tools\VsDevCmd.bat"
)

if not defined VSDEVCMD if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat" set "VSDEVCMD=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat"
if not defined VSDEVCMD if exist "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\Tools\VsDevCmd.bat" set "VSDEVCMD=C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\Tools\VsDevCmd.bat"
if not defined VSDEVCMD if exist "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat" set "VSDEVCMD=C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat"
if not defined VSDEVCMD if exist "C:\Program Files\Microsoft Visual Studio\2022\BuildTools\Common7\Tools\VsDevCmd.bat" set "VSDEVCMD=C:\Program Files\Microsoft Visual Studio\2022\BuildTools\Common7\Tools\VsDevCmd.bat"

if defined VSDEVCMD (
	call "%VSDEVCMD%" -arch=x64 -host_arch=x64 || goto :error
) else (
	echo WARNING: VsDevCmd.bat not found. Continuing with current environment.
)

set /p v=<VERSION

if "%v%"=="" (
	echo ERROR: VERSION is empty.
	goto :error
)

echo [ Compiling and Deploying v%v% ]

set app_framework=net10.0-windows
set cli_framework=net10.0
set publish_msg=Publishing Files for Setup
set compile_msg=Compiling Setup Exe

title Compiling HIRD Windows Server Setup

echo:
echo [ %publish_msg% ]
echo:

cd PowerSettings.App || goto :error
dotnet publish /p:Version=%v% --framework %app_framework% --runtime win-x64 --configuration Release --no-self-contained || goto :error
dotnet publish /p:Version=%v% --framework %app_framework% --runtime win-x86 --configuration Release --no-self-contained || goto :error

cd .. || goto :error

cd PowerSettings.CLI || goto :error
dotnet publish /p:Version=%v% --framework %cli_framework% --runtime win-x64 --configuration Release --no-self-contained || goto :error
dotnet publish /p:Version=%v% --framework %cli_framework% --runtime win-x86 --configuration Release --no-self-contained || goto :error

cd .. || goto :error

echo:
echo [ %compile_msg% ]
echo:

iscc CreateSetupx64.iss /DMyAppVersion=%v% || goto :error
iscc CreateSetupx86.iss /DMyAppVersion=%v% || goto :error

copy /Y "PowerSettings.CLI\bin\Release\%cli_framework%\win-x64\publish\powersettings.exe" "publish\powersettings.exe" >nul || goto :error
copy /Y "PowerSettings.CLI\bin\Release\%cli_framework%\win-x86\publish\powersettings.exe" "publish\powersettings_x86.exe" >nul || goto :error

echo:
echo Setup build completed successfully.
echo:

if /I not "%~1"=="--no-pause" pause

popd
endlocal
exit /b 0

:error
echo:
echo Setup failed with exit code %errorlevel%.
echo:
popd
if /I not "%~1"=="--no-pause" pause
endlocal
exit /b 1