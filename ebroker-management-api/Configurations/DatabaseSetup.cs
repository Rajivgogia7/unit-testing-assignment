using EBroker.Management.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;

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
            // To use MySQL Database
            string mySqlConnectionStr = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<EBrokerDbContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

            // To use InMemory Database
            // services.AddDbContext<EBrokerDbContext>(options => options.UseInMemoryDatabase(databaseName: "EBrokerManagementSystemDB").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        }
    }
}
