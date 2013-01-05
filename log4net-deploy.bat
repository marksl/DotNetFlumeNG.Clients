

IF %1.==. GOTO Missing
.nuget\NuGet.exe push DotNetFlumeNG.Client.log4net.%1.nupkg
.nuget\NuGet.exe push DotNetFlumeNG.Client.log4net.%1.symbols.nupkg
GOTO End


:Missing
  ECHO Missing parameter. Correct format: log4net-deploy.bat 0.1.0.0
GOTO End

:End
