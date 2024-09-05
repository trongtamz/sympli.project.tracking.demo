using MediatR;
using Sympli.Project.Tracking.Domains.Attributes;
using Sympli.Project.Tracking.Domains.Enums;
using Sympli.Project.Tracking.Domains.Responses;
using System.Text.Json.Serialization;

namespace Sympli.Project.Tracking.Domains.Requests
{
    public class GetSeoRequest : IRequest<Response<string>>
    {
        /// <summary>
        /// Keywords (Default will be: e-settlements)
        /// </summary>
        /// <example>e-settlements</example>
        [JsonPropertyName("Keywords")]
        public string? Keywords { get; set; } = string.Empty;

        /// <summary>
        /// Url (Default will be: https://www.sympli.com.au)
        /// </summary>
        /// <example>https://www.sympli.com.au</example>
        [ValidUrl]
        [JsonPropertyName("Url")]
        public string? Url { get; set; } = string.Empty;

        /// <summary>
        /// Search Engine (Default will be: Google)
        /// </summary>
        /// <example>Google</example>
        [JsonPropertyName("SearchEngine")]
        public SearchEngineEnum SearchEngine { get; set; } = SearchEngineEnum.Google;

        public GetSeoRequest()
        {
        }

        public GetSeoRequest(GetSeoRequest request, string defaultKeyword, string defaultUrl)
        {
            Keywords = string.IsNullOrEmpty(request.Keywords) ? defaultKeyword : request.Keywords;
            Url = string.IsNullOrEmpty(request.Url) ? defaultUrl : request.Url;
            SearchEngine = request.SearchEngine;
        }

        public GetSeoRequest(GetSeoMultipleEngineRequest request, SearchEngineEnum engine, string defaultKeyword, string defaultUrl)
        {
            Keywords = string.IsNullOrEmpty(request.Keywords) ? defaultKeyword : request.Keywords;
            Url = string.IsNullOrEmpty(request.Url) ? defaultUrl : request.Url;
            SearchEngine = engine;
        }
    }
}