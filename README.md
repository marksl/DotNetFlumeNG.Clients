# .Net FlumeNG Clients

Apache Flume is a distributed, reliable, and available service for efficiently collecting, aggregating, and moving large amounts of log data.

DotNetFlumeNG Clients provide targets for NLog and log4net to make connecting to flume from C# easy. 

The ThriftSource and ThriftLegacySource are both supported. For Flume 1.4+ it is recommended you use the ThriftSource.

## NLog 0.4.0.0 Installation

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

Modify your Flume .conf file. Add the ThriftSource.  

```
# test.conf
a1.channels = c1
a1.sources = r1
a1.sinks = k1

a1.channels.c1.type = memory

a1.sources.r1.channels = c1
a1.sources.r1.type = thrift
a1.sources.r1.bind = 0.0.0.0
a1.sources.r1.port = 9090

a1.sinks.k1.channel = c1
a1.sinks.k1.type = logger
```

Launch Flume from the command line. Here's an example using Java 7 Update 65 and Flume 1.5:
"C:\Program Files\Java\jdk1.7.0_65\bin\java.exe" -Dlog4j.configuration=file:///%CD%\conf\log4j.properties -cp "C:\apache-flume-1.5.0-bin\lib\*" org.apache.flume.node.Application -f C:\apache-flume-1.5.0-bin\conf\test.conf -n a1

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

## log4net 0.4.0.0 Installation

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