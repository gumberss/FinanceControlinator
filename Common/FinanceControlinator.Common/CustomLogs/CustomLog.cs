using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace FinanceControlinator.Common.CustomLogs
{
    public class CustomLog : ILogger
    {
        private CustomLogConfig _config;
        private string _flow;

        public CustomLog(String flow, CustomLogConfig config)
        {
            if (config is null)
                throw new ArgumentNullException("config", "CustomLog's config file must have a value");

            _config = config;
            _flow = flow;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                var data = new CustomLogData(
                    formatter(state, exception)
                  , logLevel
                  , eventId.Id
                  , _flow
                  , exception
                );

                Console.WriteLine(JsonSerializer.Serialize(data));
            }
        }
    }
}
