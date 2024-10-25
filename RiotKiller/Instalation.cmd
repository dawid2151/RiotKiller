@echo off
setlocal

set "serviceName=RiotKillerService"
set "exeName=RiotKiller.exe"
set "destinationFolder=C:\Program Files\RiotKiller"
set "exePath=%destinationFolder%\%exeName%"

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

endlocal
pause