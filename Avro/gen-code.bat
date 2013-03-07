REM Requirements:

REM * Java installed
REM * avro-1.7.4.jar in the current directory
REM * avro-tools-1.7.4.jar in the current directory
REM * avrogen.exe
REM * Avro.dll
REM * Newtonsoft.Json.dll

REM Run:

java -jar .\avro-tools\avro-tools-1.7.4.jar idl flume.avdl flume.avpr
.\avro-codegen\avrogen.exe -p flume.avpr ..\DotNetFlumeNG.Client.NLog\Avro
