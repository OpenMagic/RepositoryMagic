@echo off

pushd %~dp0

echo.
echo Updating RepositoryMagic project...
echo.
..\.nuget\nuget update ..\Projects\RepositoryMagic\Packages.config -Id OpenMagic -RepositoryPath ..\Packages
if not "%errorlevel%" == "0" exit %errorlevel%

echo.
echo Updating RepositoryMagic.SQL project...
echo.
..\.nuget\nuget update ..\Projects\RepositoryMagic.SQL\Packages.config -Id OpenMagic -RepositoryPath ..\Packages
if not "%errorlevel%" == "0" exit %errorlevel%

exit 0
