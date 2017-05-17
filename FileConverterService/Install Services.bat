@echo off

taskkill /f /im "FileConverterService.exe"

net stop "FileConverterService"

echo UnInstalling FileConverterService
sc delete "FileConverterService" 
echo FileConverterService
"%~dp0\FileConverterService\bin\Debug\FileConverterService.exe" install
 q
net start "FileConverterService"

pause