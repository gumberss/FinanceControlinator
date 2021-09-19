using Microsoft.Extensions.DependencyInjection;

namespace PiggyBanks.Handler.Configurations
{
    public static class AutoMapperExtension
    {
        public static void ConfigureHandlerAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperExtension));
        }
    }
}
