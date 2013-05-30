

IF %1.==. GOTO Missing
.nuget\NuGet.exe push DotNetFlumeNG.Client.NLog.%1.nupkg
.nuget\NuGet.exe push DotNetFlumeNG.Client.NLog.%1.symbols.nupkg

GOTO End


:Missing
  ECHO Missing parameter. Correct format: nlog-deploy.bat 0.1.0.0
GOTO End

:End
