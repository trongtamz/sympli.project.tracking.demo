using Sympli.Project.Tracking.Domains.Configurations;
using Sympli.Project.Tracking.Domains.Enums;
using Sympli.Project.Tracking.Infrastructures.HttpClients.Interfaces;
using Sympli.Project.Tracking.Infrastructures.HttpClients.SearchEngine;

namespace Sympli.Project.Tracking.Infrastructures.HttpClients
{
    public static class SearchEngineService
    {
        public static ISearchEngineService CreateSearchEngine(SearchEngineEnum type, HttpClient httpClient, SearchConfiguration searchConfiguration)
        {
            switch (type)
            {
                case SearchEngineEnum.Google:
                    return new GoogleEngineService(httpClient, searchConfiguration);

                case SearchEngineEnum.Bing:
                    return new BingEngineService(httpClient, searchConfiguration);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}