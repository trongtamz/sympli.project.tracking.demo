using Microsoft.Extensions.Options;
using Sympli.Project.Tracking.Applications.Interfaces;
using Sympli.Project.Tracking.Applications.Services.Base;
using Sympli.Project.Tracking.Domains.Configurations;
using Sympli.Project.Tracking.Domains.Requests;
using Sympli.Project.Tracking.Domains.Responses;
using Sympli.Project.Tracking.Infrastructures.HttpClients;
using System.Net;

namespace Sympli.Project.Tracking.Applications.Services
{
    public class SearchService : ServiceBase, ISearchService
    {
        private readonly HttpClient _httpClient;
        private readonly SearchConfiguration _searchConfiguration;

        public SearchService(HttpClient httpClient, IOptionsMonitor<SearchConfiguration> options)
        {
            _httpClient = httpClient;
            _searchConfiguration = options.CurrentValue;
        }

        public async Task<Response<string>> SearchAsync(GetSeoRequest request)
        {
            var searchEngineService = SearchEngineService.CreateSearchEngine(request.SearchEngine, _httpClient, _searchConfiguration);
            var innerContent = await searchEngineService.GetContentBlock(request.Keywords);
            if (innerContent.StatusCode != HttpStatusCode.OK)
            {
                return BuildResponse<string>(string.Empty, innerContent.StatusCode, innerContent.Message);
            }

            var results = CountPositionBaseOnResult(innerContent.Data, request.Url);

            return BuildSuccessResponse(results);
        }

        private string CountPositionBaseOnResult(IEnumerable<string> hrefs, string url)
        {
            var matched =
                hrefs.Select((href, index) => new { href, index })
                .Where(x => x.href.Contains(url))
                .Select(x => x.index + 1);
            if (matched.Any())
            {
                return string.Join(", ", matched);
            }
            return "0";
        }
    }
}