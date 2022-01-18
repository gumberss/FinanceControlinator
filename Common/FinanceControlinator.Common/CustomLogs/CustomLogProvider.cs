using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace FinanceControlinator.Common.CustomLogs
{
    public class CustomLogProvider : ILoggerProvider
    {
        private readonly CustomLogConfig _config;
        private readonly ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();

        public CustomLogProvider(CustomLogConfig config)
        {
            _config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, flow => new CustomLog(flow, _config));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
