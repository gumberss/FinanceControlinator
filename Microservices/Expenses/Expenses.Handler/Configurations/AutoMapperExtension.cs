using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Expenses.Handler.Configurations
{
    /// <summary>
    /// Must always be the last thing added in service profile
    /// </summary>
    public static class AutoMapperExtension
    {
        public static void ConfigureHandlerAutoMapper(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            var assembly = Assembly.GetAssembly(typeof(AutoMapperExtension));

            var profileTypes = assembly.GetTypes()
                .Where(x => x.IsAssignableTo(typeof(Profile)));

            var parameterLess = profileTypes
                .Where(x => x.GetConstructors().All(y => !y.GetParameters().Any()))
                .Select(Activator.CreateInstance);

            var providerParameter = profileTypes
                .Where(x => x.GetConstructors().Any(x => x.GetParameters().Any() && x.GetParameters().Any(y => y.ParameterType == typeof(IServiceProvider))))
                .Select(x => Activator.CreateInstance(x, provider));

            services.AddSingleton(s => new MapperConfiguration(conf =>
            parameterLess.Concat(providerParameter)
                .ToList()
                .ForEach(x => conf.AddProfile((Profile)x)))
            .CreateMapper());
        }
    }
}
