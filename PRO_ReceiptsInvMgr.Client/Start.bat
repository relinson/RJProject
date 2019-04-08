@echo off
 
if  exist "%windir%\Microsoft.NET\Framework\v2.0*" start "" "C:\Users\kery.chen\Desktop\PreInstall1.0\PreInstall1.0\PreInstall.Client.exe" && exit
if  exist "%windir%\Microsoft.NET\Framework\v4.0*" start "" "C:\Users\kery.chen\Desktop\PreInstall1.0\PreInstall1.0\PreInstall.Client.exe" && exit
start "" "C:\Users\kery.chen\Desktop\PreInstall1.0\PreInstall1.0\SetUp\dotNetFx40_Full_x86_x64.exe"
:loop
if exist "%windir%\Microsoft.NET\Framework\v4.0*" start "" "C:\Users\kery.chen\Desktop\PreInstall1.0\PreInstall1.0\PreInstall.Client.exe" && exit
goto :loop