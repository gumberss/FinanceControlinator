using Microsoft.Extensions.DependencyInjection;
using PiggyBanks.Handler;

namespace PiggyBanks.API.Commons
{
    public static class AutoMapperExtension
    {
        public static void ConfigureHandlerAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperExtension));
            services.AddAutoMapper(typeof(HandlerModule));
        }
    }
}
