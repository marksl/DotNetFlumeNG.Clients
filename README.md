# .Net FlumeNG Clients Pre-Release!

Apache Flume is a distributed, reliable, and available service for efficiently collecting, aggregating, and moving large amounts of log data. DotNet

This is a pre-release of DotNetFlumeNG Clients that uses Apache Avro as a transport instead of Apache Thrift. Currently only NLog is supported. This
uses a forked copy of Apache Avro with the Avro-975-complete5.patch from here https://issues.apache.org/jira/browse/AVRO-975 applied.

## NLog 0.3.2.0 Installation

Type the following in the Visual Studio Package Manager Console.  

```
Install-Package DotNetFlumeNG.Client.NLog -Pre
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
    
	<default-wrapper xsi:type="AsyncWrapper"
                   queueLimit="100"
                   overflowAction="Discard">
    </default-wrapper>

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

Modify your Flume .conf file. Add the Avro source.  

```
agent.sources = avrosource
agent.channels = memoryChannel-1
agent.sinks = Console

agent.sources.avrosource.type = avro
agent.sources.avrosource.host = localhost
agent.sources.avrosource.port = 9090
agent.sources.avrosource.channels = memoryChannel-1

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
