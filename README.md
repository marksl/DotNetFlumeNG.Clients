# .Net FlumeNG Clients

Apache Flume is a distributed, reliable, and available service for efficiently collecting, aggregating, and moving large amounts of log data. DotNet

DotNetFlumeNG Clients provide targets for NLog and log4net to make connecting to flume from C# easy. It uses the Flume legacy thrift support because currently C# Avro RPC support is lacking.

## NLog 0.1.1.0 Installation

1.  Type the following in the Visual Studio Package Manager Console.  
```
Install-Package DotNetFlumeNG.Client.NLog
```

2.  Add the following configuration to your web.config or app.config:

```
<configuration>
  
  <configSections>
    <section name="nlog" type="NLog.Config.ConfigSectionHandler, NLog" />
  </configSections>

  <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <extensions>
      <add assembly="DotNetFlumeNG.Client.NLog" />
    </extensions>
    <targets>
      <target name="a1" type="Flume" >
        <source host="localhost" port="9090"></source>
      </target>
    </targets>
    <rules>
      <logger name="*" minLevel="Info" appendTo="a1" />
    </rules>
  </nlog>

</configuration>
```
3.  Modify your Flume .conf file. Add the ThriftLegacySource.  

```
agent.sources = legacysource-1
agent.channels = memoryChannel-1
agent.sinks = Console

agent.sources.legacysource-1.type = org.apache.flume.source.thriftLegacy.ThriftLegacySource
agent.sources.legacysource-1.host = localhost
agent.sources.legacysource-1.port = 9090
agent.sources.legacysource-1.channels = memoryChannel-1

agent.channels.memoryChannel-1.type = memory

agent.sinks.Console.channel = memoryChannel-1
agent.sinks.Console.type = logger
```
Note: A good site for how to install Flume on windows can be found here: 
http://mapredit.blogspot.ca/2012/07/run-flume-13x-on-windows.html  
4.  Write NLog logging code as usual  

```
using NLog;
 
public class MyClass
{
	private static Logger logger = LogManager.GetCurrentClassLogger();
 
	public void MyMethod1()
	{
      logger.Trace("Sample trace message");
      logger.Debug("Sample debug message");
      logger.Info("Sample informational message");
	}
}
```

## log4net

Coming soon.