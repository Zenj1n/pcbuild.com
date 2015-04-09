@echo off
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment" /v R_HOME /d C:\Program Files\R\R-3.1.3\" /f
echo Het "Environment Variables" Windows is geopent..."
echo klik op "OK"
pause