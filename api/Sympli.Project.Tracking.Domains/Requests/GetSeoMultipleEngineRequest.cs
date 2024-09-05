using MediatR;
using Sympli.Project.Tracking.Domains.Attributes;
using Sympli.Project.Tracking.Domains.Enums;
using Sympli.Project.Tracking.Domains.Responses;
using System.Text.Json.Serialization;

namespace Sympli.Project.Tracking.Domains.Requests
{
    public class GetSeoMultipleEngineRequest : IRequest<Response<IEnumerable<string>>>
    {
        /// <summary>
        /// Keywords (Default will be: e-settlements)
        /// </summary>
        /// <example></example>
        [JsonPropertyName("Keywords")]
        public string Keywords { get; set; } = string.Empty;

        /// <summary>
        /// Url (Default will be: https://www.sympli.com.au)
        /// </summary>
        /// <example></example>
        [ValidUrl]
        [JsonPropertyName("Url")]
        public string Url { get; set; } = string.Empty;

        [NotEmpty]
        [JsonPropertyName("Engines")]
        public IEnumerable<SearchEngineEnum> SearchEngines { get; set; }

        public GetSeoMultipleEngineRequest()
        {
        }

        public GetSeoMultipleEngineRequest(GetSeoMultipleEngineRequest request, string defaultKeyword, string defaultUrl)
        {
            Keywords = string.IsNullOrEmpty(request.Keywords) ? defaultKeyword : request.Keywords;
            Url = string.IsNullOrEmpty(request.Url) ? defaultUrl : request.Url;
        }
    }
}