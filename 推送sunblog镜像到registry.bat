@echo off
cd /d %~dp0

echo "push images start"

pwsh .\push_sunblog_imgs.ps1

echo "push images end"
pause
