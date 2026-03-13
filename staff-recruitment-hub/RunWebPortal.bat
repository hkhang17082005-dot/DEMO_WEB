@echo off
chcp 65001 > nul
echo Run WebPortal...

cd src/SRB_WebPortal
call dotnet watch run --no-launch-browser --no-https
echo.
echo Run WebPortal Error...
pause
