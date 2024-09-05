using Sympli.Project.Tracking.Domains;
using Sympli.Project.Tracking.Domains.Configurations;
using Sympli.Project.Tracking.Domains.Responses;
using Sympli.Project.Tracking.Infrastructures.HttpClients.Interfaces;
using System.Net;
using System.Text.RegularExpressions;

namespace Sympli.Project.Tracking.Infrastructures.HttpClients.SearchEngine
{
    public class GoogleEngineService : ISearchEngineService
    {
        private readonly HttpClient _httpClient;
        private readonly SearchConfiguration _searchConfiguration;

        public GoogleEngineService()
        {
        }

        public GoogleEngineService(HttpClient httpClient, SearchConfiguration searchConfiguration)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", searchConfiguration.DefaultUserAgent ?? Constants.UserAgent);
            _searchConfiguration = searchConfiguration;
        }

        public async Task<Response<IEnumerable<string>>> GetContentBlock(string keywords)
        {
            var url = CommonUtils.GetDefaulGoogleSearch(_searchConfiguration.DefaultUrlSearchGoogle,
                keywords, _searchConfiguration.DefaultTotalItem);
            var htmlContent = await url.TryReadContentAsString(_httpClient);

            if (string.IsNullOrEmpty(htmlContent))
            {
                return new Response<IEnumerable<string>>
                {
                    Data = Enumerable.Empty<string>(),
                    StatusCode = HttpStatusCode.NotFound,
                    Message = string.Format(SympliTrackingMessage.Error.ErrorReadUrl, url)
                };
            }

            string innerContent = GetInnerContent(htmlContent);
            if (string.IsNullOrEmpty(innerContent))
            {
                return new Response<IEnumerable<string>>
                {
                    Data = Enumerable.Empty<string>(),
                    StatusCode = HttpStatusCode.NoContent,
                    Message = string.Format(SympliTrackingMessage.Error.ErrorNotFoundResultUrl, url)
                };
            }

            var results = GetTagHrefs(innerContent);
            return new Response<IEnumerable<string>>
            {
                Data = results,
                StatusCode = HttpStatusCode.OK,
                Message = "Success"
            };
        }

        private string GetInnerContent(string htmlContent)
        {
            // Results inside tag div id rso
            string pattern = Constants.PatternResultGoogleSearch;
            Match match = Regex.Match(htmlContent, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (match.Success)
            {
                // Extract the inner HTML
                string innerContent = match.Groups[0].Value;
                return innerContent;
            }
            else
            {
                return string.Empty;
            }
        }

        private IEnumerable<string> GetTagHrefs(string htmlContent)
        {
            List<string> anchors = new List<string>();

            // Get url on href tag
            string anchorPattern = Constants.PatternHrefTagGoogleSearch;
            Regex anchorRegex = new Regex(anchorPattern, RegexOptions.Singleline);

            MatchCollection matches = anchorRegex.Matches(htmlContent);
            var removeTranslateUrls = matches.Where(m => !m.Groups[1].Value.Contains("translate.google.com"));
            foreach (Match match in removeTranslateUrls)
            {
                string anchor = match.Groups[1].Value;
                anchors.Add(anchor);
            }

            return anchors.Take(_searchConfiguration.DefaultTotalItem);
        }
    }
}