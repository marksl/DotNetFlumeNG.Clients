

IF %1.==. GOTO Missing
.nuget\NuGet.exe push DotNetFlumeNG.Client.NLog.%1.nupkg -s https://nuget.globalrelay.net/ 6467ce88-104a-4d1b-9068-db0dba810b93
GOTO End


:Missing
  ECHO Missing parameter. Correct format: nlog-deploy.bat 0.1.0.0
GOTO End

:End
