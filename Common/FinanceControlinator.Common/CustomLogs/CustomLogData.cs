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
          , int eventId
          , string flow
          , Exception exception
        )
        {
            Message = message;
            this.logLevel = logLevel.ToString();
            this.eventId = eventId;
            Flow = flow;
            Exception = exception?.ToString();
        }

        public String Message { get; set; }

        public String logLevel { get; set; }

        public int eventId { get; set; }

        public String Flow { get; set; }

        public String Exception { get; set; }
    }
}
