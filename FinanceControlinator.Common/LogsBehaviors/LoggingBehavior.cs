using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.LogsBehaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var response = await next();

            stopwatch.Stop();

            _logger.LogInformation($"Handled {typeof(TRequest).Name} with {typeof(TResponse).Name} in {stopwatch.ElapsedMilliseconds}");

            return response;
        }
    }
}
