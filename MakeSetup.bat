@echo off
setlocal EnableExtensions

pushd "%~dp0" || goto :error

if not exist "VERSION" (
	echo ERROR: VERSION file not found.
	goto :error
)

call "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat" -arch=x64 -host_arch=x64 || goto :error
set /p v=<VERSION

if "%v%"=="" (
	echo ERROR: VERSION is empty.
	goto :error
)

echo [ Compiling and Deploying v%v% ]

set publish_msg=Publishing Files for Setup
set compile_msg=Compiling Setup Exe

title Compiling HIRD Windows Server Setup

echo:
echo [ %publish_msg% ]
echo:

cd PowerSettings.App || goto :error
dotnet publish /p:Version=%v% --framework net7.0-windows --runtime win-x64 --configuration Release --no-self-contained || goto :error
dotnet publish /p:Version=%v% --framework net7.0-windows --runtime win-x86 --configuration Release --no-self-contained || goto :error

cd .. || goto :error

cd PowerSettings.CLI || goto :error
dotnet publish /p:Version=%v% --framework net7.0 --runtime win-x64 --configuration Release --no-self-contained || goto :error
dotnet publish /p:Version=%v% --framework net7.0 --runtime win-x86 --configuration Release --no-self-contained || goto :error

cd .. || goto :error

echo:
echo [ %compile_msg% ]
echo:

iscc CreateSetupx64.iss /DMyAppVersion=%v% || goto :error
iscc CreateSetupx86.iss /DMyAppVersion=%v% || goto :error

copy /Y "PowerSettings.CLI\bin\Release\net7.0\win-x64\publish\powersettings.exe" "publish\powersettings.exe" >nul || goto :error
copy /Y "PowerSettings.CLI\bin\Release\net7.0\win-x86\publish\powersettings.exe" "publish\powersettings_x86.exe" >nul || goto :error

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