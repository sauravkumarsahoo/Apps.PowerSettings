@echo off
call "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat" -arch=x64 -host_arch=x64
set /p v=<VERSION

echo [ Compiling and Deploying v%v% ]

set pulling_msg=Pulling any Unsynced Changes
set publish_msg=Publishing Files for Setup
set compile_msg=Compiling Setup Exe
set deploy_msg=Deploying Release v%v%

title Compiling HIRD Windows Server Setup

echo:
echo [ %pulling_msg% ]
echo:

cd src\HIRD
echo:
echo [ %publish_msg% ]
echo:

dotnet publish /p:Version=%v% --framework net6.0-windows --runtime win-x64 --configuration Release --no-self-contained
dotnet publish /p:Version=%v% --framework net6.0-windows --runtime win-x86 --configuration Release --no-self-contained

cd ..\..

echo:
echo [ %compile_msg% ]
echo:

iscc CreateSetupx64.iss /DMyAppVersion=%v%
iscc CreateSetupx86.iss /DMyAppVersion=%v%
echo:
echo [ %deploy_msg% ]
echo:

::git add .
::git commit -m "%deploy_msg%"
::git tag v%v%
::git push origin
::git push origin --tags

echo:
echo:

pause