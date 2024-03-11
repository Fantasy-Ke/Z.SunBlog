chcp 65001
@echo off
cd /d %~dp0

echo "build images start"
echo "安装PowerShell7！！！！！！"

pwsh .\build_sunblog_imgs.ps1

echo "build images end"
pause
