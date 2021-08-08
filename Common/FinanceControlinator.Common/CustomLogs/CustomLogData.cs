using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.CustomLogs
{
    public class CustomLogData
    {
        public CustomLogData(
            string message
          , LogLevel logLevel
          , EventId eventId
          , string flow
          , Exception exception
        )
        {
            Message = message;
            this.logLevel = logLevel;
            this.eventId = eventId;
            Flow = flow;
            Exception = exception;
        }

        public String Message { get; set; }

        public LogLevel logLevel { get; set; }

        public EventId eventId { get; set; }

        public String Flow { get; set; }

        public Exception Exception { get; set; }
    }
}
