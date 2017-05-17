@echo off

taskkill /f /im "FileConverterService.exe"

net stop "FileConverterService"

net start "FileConverterService"

pause