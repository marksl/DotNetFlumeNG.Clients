using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Core;

namespace DotNetFlumeNG.Client.log4net
{
    public class Class1
    {
        public Class1()
        {
            var to = LogManager.GetLogger("asfsadf");

            //LoggingEventData data = new LoggingEventData();
            //data.Properties
            //var even = new LoggingEvent(data);
            //to.Logger.Log();
        }
        
    }
}
