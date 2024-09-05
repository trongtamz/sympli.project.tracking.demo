using Microsoft.Extensions.DependencyInjection;
using Sympli.Project.Tracking.Infrastructures.Cache;
using Sympli.Project.Tracking.Infrastructures.Cache.Interfaces;
using Sympli.Project.Tracking.Infrastructures.HttpClients.Interfaces;
using Sympli.Project.Tracking.Infrastructures.HttpClients.SearchEngine;

namespace Sympli.Project.Tracking.Infrastructures
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEngineService(this IServiceCollection services)
        {
            services.AddScoped<ISearchEngineService, GoogleEngineService>();
            services.AddScoped<ISearchEngineService, BingEngineService>();
        }

        public static void AddCacheService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}