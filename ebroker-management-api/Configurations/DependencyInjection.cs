using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EBroker.Management.Application;
using EBroker.Management.Application.Infrastructure;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using EBroker.Management.Data;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Api.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            ApplicationSettings appConfig = new ApplicationSettings();
            configuration.Bind("ApplicationSettings", appConfig);
            services.AddSingleton(appConfig);

            services.AddScoped<IEBrokerDbContext, EBrokerDbContext>();

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddApplication();
            return services;
        }
    }
}
