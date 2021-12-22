using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using EBroker.Management.Application.Infrastructure.AutoMapper;
using EBroker.Management.Application.Shared;
using EBroker.Management.Application.Traders;
using EBroker.Management.Application.Equities;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationSettings).Assembly);
            ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<ITraderService, TraderService>();
            services.AddTransient<IEquityService, EquityService>();
        }
    }
}
