# .Net FlumeNG Clients

Apache Flume is a distributed, reliable, and available service for efficiently collecting, aggregating, and moving large amounts of log data. DotNet

DotNetFlumeNG Clients provide targets for NLog and log4net to make connecting to flume from C# easy. It uses the Flume legacy thrift support because currently C# Avro RPC support is lacking.

## NLog 0.2.3.0 Installation

Type the following in the Visual Studio Package Manager Console.  

```
Install-Package DotNetFlumeNG.Client.NLog
```

Add the following configuration to your web.config or app.config:

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
      <target name="a1" type="Flume" host="localhost" port="9090" />
    </targets>
    <rules>
      <logger name="*" minLevel="Info" appendTo="a1" />
    </rules>
  </nlog>

</configuration>
```

Multiple flume sources are supported using NLog's Round Robin wrapper.
```
<target type="RoundRobinGroup" name="MultipleFlumes">
    <target name="a1" type="Flume" host="localhost" port="9090" />
    <target name="a2" type="Flume" host="localhost" port="9090" />
    <target name="a3" type="Flume" host="localhost" port="9090" />
</target>
```

Modify your Flume .conf file. Add the ThriftLegacySource.  

```
agent.sources = legacysource-1
agent.channels = memoryChannel-1
agent.sinks = Console

# For Flume 1.3 and earlier use agent.sources.legacysource-1.type = org.apache.flume.source.thriftLegacy.ThriftLegacySource
agent.sources.legacysource-1.type = thrift
agent.sources.legacysource-1.host = localhost
agent.sources.legacysource-1.port = 9090
agent.sources.legacysource-1.channels = memoryChannel-1

agent.channels.memoryChannel-1.type = memory

agent.sinks.Console.channel = memoryChannel-1
agent.sinks.Console.type = logger
```
Note: A good site for how to install Flume on windows can be found here: 
http://mapredit.blogspot.ca/2012/07/run-flume-13x-on-windows.html  

Write NLog logging code as usual  

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

## log4net 0.2.3.0 Installation

Type the following in the Visual Studio Package Manager Console.  

```
Install-Package DotNetFlumeNG.Client.log4net
```

Add the following configuration to your web.config or app.config:

```
<configuration>

  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>
    
  <log4net>
    
    <appender name="FlumeAppender" type="DotNetFlumeNG.Client.log4net.FlumeAppender, DotNetFlumeNG.Client.log4net">
      <client>Thrift</client>
      <host>localhost</host>
      <port>9090</port>
      <layout type="log4net.Layout.PatternLayout">
        <ConversionPattern value="%m" />
      </layout>
    </appender>
    
    <root>
      <level value="INFO"/>
      <appender-ref ref="FlumeAppender" />
    </root>
  
  </log4net>

</configuration>
```

Modify your Flume .conf file as documented above for NLog.

Write log4net code as usual:

```
using log4net;
using log4net.Config;
 
public class MyClass
{
	private static readonly ILog logger = LogManager.GetLogger(typeof (MyClass));
 
	public void MyMethod1()
	{
        XmlConfigurator.Configure();

        logger.Debug("Sample debug message");
        logger.Info("Sample informational message");
	}
}
```