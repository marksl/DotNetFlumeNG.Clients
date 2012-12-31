using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace DotNetFlumeNG.Client.log4net
{
    public class FlumeAppender : AppenderSkeleton
    {

        protected override void Append(LoggingEvent loggingEvent)
        {
            throw new NotImplementedException();
        }
    }
}
