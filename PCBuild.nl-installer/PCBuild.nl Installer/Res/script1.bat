@echo off
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment" /v PATH /d "%path%;C:\Python27;C:\Python27\Scripts;C:\Program Files\R\R-3.1.3\bin" /f
echo Het "Environment Variables" Windows is geopent..."
echo klik op "OK"
pause