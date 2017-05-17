@echo off

taskkill /f /im "FileConverterService.exe"

net stop "FileConverterService"

echo UnInstalling FileConverterService
sc delete "FileConverterService" 

pause