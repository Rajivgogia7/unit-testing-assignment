using EBroker.Management.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EBroker.Management.Api.Configurations
{
    public static class DatabaseSetup
    {
        public static void AddDatabaseSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddDbContext<EBrokerDbContext>(options => options.UseInMemoryDatabase(databaseName: "EBrokerManagementSystemDB").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        }
    }
}
