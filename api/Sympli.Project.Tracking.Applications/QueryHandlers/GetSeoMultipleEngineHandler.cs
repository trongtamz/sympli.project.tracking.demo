using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Sympli.Project.Tracking.Applications.Interfaces;
using Sympli.Project.Tracking.Domains;
using Sympli.Project.Tracking.Domains.Configurations;
using Sympli.Project.Tracking.Domains.Requests;
using Sympli.Project.Tracking.Domains.Responses;
using Sympli.Project.Tracking.Infrastructures.Cache.Interfaces;
using System.Net;

namespace Sympli.Project.Tracking.Applications.QueryHandlers
{
    public class GetSeoMultipleEngineHandler : IRequestHandler<GetSeoMultipleEngineRequest, Response<IEnumerable<string>>>
    {
        private readonly ISearchService _searchService;
        private readonly ICacheService _cacheService;
        private readonly SearchConfiguration _searchConfiguration;

        public GetSeoMultipleEngineHandler(ISearchService searchService,
            ICacheService cacheService,
            IOptionsMonitor<SearchConfiguration> searchConfiguration)
        {
            _searchService = searchService;
            _cacheService = cacheService;
            _searchConfiguration = searchConfiguration.CurrentValue;
        }

        public async Task<Response<IEnumerable<string>>> Handle(GetSeoMultipleEngineRequest request, CancellationToken cancellationToken)
        {
            var engines = request.SearchEngines.Distinct().OrderBy(e => e).ToList();

            var cacheKey = string.Format(Constants.CachedKeySearchingSEO, string.Join(",", engines));
            if (!_cacheService.TryGetValue(cacheKey, out List<string> cachedValue))
            {
                cachedValue = new List<string>();
                foreach (var engine in engines)
                {
                    var initDefaultRequestIfEmpty = new GetSeoRequest(request, engine, _searchConfiguration.DefaultKeyword, _searchConfiguration.DefaultUrl);
                    var response = await _searchService.SearchAsync(initDefaultRequestIfEmpty);

                    if (string.IsNullOrEmpty(response.Data) || response.StatusCode != HttpStatusCode.OK)
                    {
                        return new Response<IEnumerable<string>>
                        {
                            Data = Enumerable.Empty<string>(),
                            StatusCode = response.StatusCode,
                            Message = response.Message
                        };
                    }

                    cachedValue.Add(string.Format(SympliTrackingMessage.Success.SuccessSearching,
                        initDefaultRequestIfEmpty.Keywords, engine.ToString(), response.Data));
                }

                if (cachedValue.Count == 0)
                {
                    return new Response<IEnumerable<string>>
                    {
                        Data = Enumerable.Empty<string>(),
                        StatusCode = HttpStatusCode.NoContent,
                        Message = SympliTrackingMessage.Error.ErrorValueEmptyReturn
                    };
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                _cacheService.Set(cacheKey, cachedValue, cacheEntryOptions);
            }

            return new Response<IEnumerable<string>>
            {
                Data = cachedValue,
                StatusCode = HttpStatusCode.OK,
                Message = "Success"
            };
        }
    }
}