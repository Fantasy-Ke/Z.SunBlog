chcp 65001
@echo off
cd /d %~dp0

echo "push images start"
echo "安装PowerShell7！！！！！！"

pwsh .\push_sunblog_imgs.ps1

echo "push images end"
pause
