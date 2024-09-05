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
    public class GetSeoHandler : IRequestHandler<GetSeoRequest, Response<string>>
    {
        private readonly ISearchService _searchService;
        private readonly ICacheService _cacheService;
        private readonly SearchConfiguration _searchConfiguration;

        public GetSeoHandler(ISearchService searchService,
            ICacheService cacheService,
            IOptionsMonitor<SearchConfiguration> searchConfiguration)
        {
            _searchService = searchService;
            _cacheService = cacheService;
            _searchConfiguration = searchConfiguration.CurrentValue;
        }

        public async Task<Response<string>> Handle(GetSeoRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(Constants.CachedKeySearchingSEO, 
                request.Keywords,
                request.Url,
                request.SearchEngine.ToString());
            if (!_cacheService.TryGetValue(cacheKey, out string cachedValue))
            {
                var initDefaultRequestIfEmpty = new GetSeoRequest(request, _searchConfiguration.DefaultKeyword, _searchConfiguration.DefaultUrl);
                var response = await _searchService.SearchAsync(initDefaultRequestIfEmpty);

                if (string.IsNullOrEmpty(response.Data) || response.StatusCode != HttpStatusCode.OK)
                {
                    return response;
                }
                cachedValue = string.Format(SympliTrackingMessage.Success.SuccessSearching,
                    initDefaultRequestIfEmpty.Keywords, initDefaultRequestIfEmpty.SearchEngine.ToString(), response.Data);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };

                _cacheService.Set(cacheKey, cachedValue, cacheEntryOptions);
            }

            return new Response<string>
            {
                Data = cachedValue,
                StatusCode = HttpStatusCode.OK,
                Message = "Success"
            };
        }
    }
}