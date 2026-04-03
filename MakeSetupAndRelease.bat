@echo off
setlocal EnableExtensions

pushd "%~dp0" || goto :error

if not exist "VERSION" (
	echo ERROR: VERSION file not found.
	goto :error
)

set /p v=<VERSION

if "%v%"=="" (
	echo ERROR: VERSION is empty.
	goto :error
)

set deploy_msg=Deploying Release v%v%

title Compiling HIRD Windows Server Setup

echo:
echo [ Running setup build ]
echo:

call "%~dp0MakeSetup.bat" --no-pause || goto :error

echo:
echo [ %deploy_msg% ]
echo:

git add . || goto :error
git commit -m "%deploy_msg%" || goto :error
git tag v%v% || goto :error
git push origin || goto :error
git push origin --tags || goto :error

echo:
echo Release completed successfully.
echo:

popd
endlocal
pause
exit /b 0

:error
echo:
echo Release failed with exit code %errorlevel%.
echo:
popd
endlocal
pause
exit /b 1