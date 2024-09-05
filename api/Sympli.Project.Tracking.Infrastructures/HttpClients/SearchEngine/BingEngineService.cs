using Sympli.Project.Tracking.Domains;
using Sympli.Project.Tracking.Domains.Configurations;
using Sympli.Project.Tracking.Domains.Responses;
using Sympli.Project.Tracking.Infrastructures.HttpClients.Interfaces;
using System.Net;
using System.Text.RegularExpressions;

namespace Sympli.Project.Tracking.Infrastructures.HttpClients.SearchEngine
{
    public class BingEngineService : ISearchEngineService
    {
        private readonly HttpClient _httpClient;
        private readonly SearchConfiguration _searchConfiguration;
        private const int DefaultItemPerPage = 10;

        public BingEngineService()
        {
        }

        public BingEngineService(HttpClient httpClient, SearchConfiguration searchConfiguration)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", searchConfiguration.DefaultUserAgent ?? Constants.UserAgent);
            _searchConfiguration = searchConfiguration;
        }

        public async Task<Response<IEnumerable<string>>> GetContentBlock(string keywords)
        {
            var innerContents = new List<string>();
            var page = _searchConfiguration.DefaultTotalItem / DefaultItemPerPage;
            for (int i = 0; i < page; i++)
            {
                var url = CommonUtils.GetDefaulBingSearch(_searchConfiguration.DefaultUrlSearchBing,
                    keywords, i, DefaultItemPerPage);
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

                var innerContent = GetInnerContent(htmlContent);
                if (!string.IsNullOrEmpty(innerContent))
                {
                    innerContents.Add(innerContent);
                }
            }

            var results = GetTagCites(innerContents);

            return new Response<IEnumerable<string>>
            {
                Data = results,
                StatusCode = HttpStatusCode.OK,
                Message = "Success"
            };
        }

        private string GetInnerContent(string htmlContent)
        {
            // Results inside tag ol id b_results
            string pattern = Constants.PatternResultBingSearch;
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

        private IEnumerable<string> GetTagCites(IEnumerable<string> innerContents)
        {
            List<string> anchors = new List<string>();
            // Get url on cite tag
            string anchorPattern = Constants.PatternCiteTagBingSearch;
            Regex anchorRegex = new Regex(anchorPattern, RegexOptions.Singleline);

            foreach (string innerContent in innerContents)
            {
                MatchCollection matches = anchorRegex.Matches(innerContent);
                foreach (Match match in matches)
                {
                    string anchor = match.Groups[1].Value;
                    anchors.Add(anchor);
                }
            }

            return anchors.Take(_searchConfiguration.DefaultTotalItem);
        }
    }
}