@echo off
echo Install Packages Project

echo [1/3] Install NuGet Packages
dotnet restore
if %errorlevel% neq 0 (
   echo [Error] Restore NuGet Packages Error
   pause
   exit /b %errorlevel%
)

echo [2/3] Install Node modules [Tailwind CSS]
cd src/SRB_WebPortal
call npm install
if %errorlevel% neq 0 (
   echo [Error] Install npm Packages Error
   pause
   exit /b %errorlevel%
)

echo [3/3] Build Project First...
dotnet build --no-restore
if %errorlevel% neq 0 (
   echo [Error] Build Project Error
   pause
   exit /b %errorlevel%
)

echo Install Packages Project Success
pause
