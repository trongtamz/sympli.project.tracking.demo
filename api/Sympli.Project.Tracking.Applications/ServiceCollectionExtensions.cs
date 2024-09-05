using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sympli.Project.Tracking.Applications.Interfaces;
using Sympli.Project.Tracking.Applications.Services;
using Sympli.Project.Tracking.Applications.Validators;
using Sympli.Project.Tracking.Domains.Configurations;
using Sympli.Project.Tracking.Infrastructures;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Sympli.Project.Tracking.Applications
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            services.AddConfiguration(configuration);
            var thisAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(thisAssembly));
            services.AddServices();
            services.AddHttpClient();
            services.AddInfrastructures();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressMapClientErrors = true;
            });

            services.AddControllers(c => { c.Conventions.Add(new SwaggerConventions(configuration)); });
            services.AddSwaggerGen(c =>
            {
                var version = configuration.GetValue<string>("Version");

                c.SwaggerDoc($"v{version}",
                    new OpenApiInfo { Title = "Sympli Project Tracking Api", Version = $"v{version}" });
                c.EnableAnnotations();
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.GetName().Name?.StartsWith("Sympli") ?? false);
                foreach (var assembly in assemblies)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
            });
        }

        private static void AddHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<SearchService>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            // Transient services
            services.AddTransient(typeof(IRequestValidator<>), typeof(RequestValidator<>));

            services.AddScoped<ISearchService, SearchService>();
            //services.AddSingleton<IInMemoryCacheService, InMemoryCacheService>();
        }

        private static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SearchConfiguration>(configuration.GetSection("SearchTool"));
        }

        private static void AddInfrastructures(this IServiceCollection services)
        {
            services.AddEngineService();
            services.AddCacheService();
        }
    }
}