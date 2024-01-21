@echo off
cd /d %~dp0

echo "build images start"

pwsh .\build_sunblog_imgs.ps1

echo "build images end"
pause
