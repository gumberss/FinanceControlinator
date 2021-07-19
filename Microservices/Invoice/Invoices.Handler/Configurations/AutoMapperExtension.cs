using Microsoft.Extensions.DependencyInjection;

namespace Invoices.Handler.Configurations
{
    public static class AutoMapperExtension
    {
        public static void ConfigureHandlerAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperExtension));
        }
    }
}
