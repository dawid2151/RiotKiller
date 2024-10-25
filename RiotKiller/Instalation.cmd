@echo off
setlocal

REM Checking for administrator rights
net session >nul 2>&1
    if %errorLevel% NEQ 0 (
        echo Script has to be run as an Administrator
        goto end
    )


:installation
set "serviceName=RiotKillerService"
set "exeName=RiotKiller.exe"
set "destinationFolder=C:\Program Files\RiotKiller"
set "exePath=%destinationFolder%\%exeName%"

REM Stop service if it runs
echo Stopping service %serviceName%...
sc stop "%serviceName%"
timeout /t 5 >nul

REM Step 1: Copy the executable to the destination folder
echo Copying %exeName% to %destinationFolder%...
if not exist "%destinationFolder%" (
    mkdir "%destinationFolder%"
)
copy /Y "%~dp0%exeName%" "%exePath%"

REM Step 2: Create the service
echo Creating Windows Service named %serviceName%...
sc create "%serviceName%" binPath= "\"%exePath%\"" start= auto displayname= "Riot Killer Service"

REM Step 3: Start the service
echo Starting the service %serviceName%...
sc start "%serviceName%"

echo Installation of %serviceName% was successful! You can delete installation files

:end
endlocal
pause