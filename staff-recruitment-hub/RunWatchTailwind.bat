@echo off
chcp 65001 > nul
echo Run Watch Change Tailwind...

cd src/SRB_WebPortal
call npx @tailwindcss/cli -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --watch
echo.
echo Run Watch Tailwind Error...
pause
