using Microsoft.Extensions.Logging;

namespace FinanceControlinator.Common.CustomLogs
{
    public class CustomLogConfig
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;
    }
}
