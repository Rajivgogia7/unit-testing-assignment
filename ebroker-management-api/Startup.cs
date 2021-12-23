using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using EBroker.Management.Api.Configurations;
using EBroker.Management.Api.RequestHeaderValidation;
using Serilog;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddDatabaseSetup(Configuration);

            services.Configure<AppSettings>(options =>
            {
                Configuration.GetSection("AppSettings").Bind(options);
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSingleton(resolver =>
            {
                return resolver.GetRequiredService<IOptions<AppSettings>>().Value;
            });

            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddHttpClient();

            services.AddLogging((builder) =>
            {
                builder.AddSerilog(dispose: true);
            });

            //Update routes to lower case 
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.RegisterDependencies(Configuration);

            services.AddScoped<InternalAPIRequiredHeadersAttribute>();

            // Swagger Config
            services.AddSwaggerSetup();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwaggerSetup();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().WithMetadata();
            });
        }
    }
}
