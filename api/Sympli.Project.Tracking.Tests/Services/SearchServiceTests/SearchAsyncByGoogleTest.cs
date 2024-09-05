using Microsoft.Extensions.Options;
using Moq;
using Sympli.Project.Tracking.Applications.Services;
using Sympli.Project.Tracking.Domains;
using Sympli.Project.Tracking.Domains.Configurations;
using Sympli.Project.Tracking.Domains.Requests;
using System.Net;

namespace Sympli.Project.Tracking.Tests.Services.SearchServiceTests
{
    public class SearchAsyncByGoogleTest : BaseUnitTest
    {
        [Fact]
        public async Task SearchAsync_Expect_Success()
        {
            // Arrange
            var request = new GetSeoRequest
            {
                Keywords = "e-settlements",
                Url = "https://www.sympli.com.au",
                SearchEngine = Domains.Enums.SearchEngineEnum.Google
            };

            var httpClient = new Mock<HttpClient>();
            var searchService = new SearchService(httpClient.Object, SearchConfiguration);

            // Action
            var result = await searchService.SearchAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(!string.IsNullOrEmpty(result.Data));
        }

        [Fact]
        public async Task SearchAsync_Expect_SuccessWithNoLimit()
        {
            // Arrange
            var request = new GetSeoRequest
            {
                Keywords = "Sample",
                Url = "https://www.sympli.com.au",
                SearchEngine = Domains.Enums.SearchEngineEnum.Google
            };
            var searchConfiguration = new Mock<IOptionsMonitor<SearchConfiguration>>();
            searchConfiguration.Setup(x => x.CurrentValue).Returns(new SearchConfiguration
            {
                DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
                DefaultKeyword = "e-settlements",
                DefaultUrl = "https://www.sympli.com.au",
                DefaultTotalItem = 0,
                DefaultUrlSearchGoogle = "https://www.google.com.au",
                DefaultUrlSearchBing = "https://www.bing.com"
            });
            var httpClient = new Mock<HttpClient>();
            var searchService = new SearchService(httpClient.Object, searchConfiguration.Object);

            // Action
            var result = await searchService.SearchAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(!string.IsNullOrEmpty(result.Data));
        }

        [Fact]
        public async Task SearchAsync_Expect_InvalidUrlSearch()
        {
            // Arrange
            var request = new GetSeoRequest
            {
                Keywords = "Sample",
                Url = "sympli.com.au",
                SearchEngine = Domains.Enums.SearchEngineEnum.Google
            };

            var httpClient = new Mock<HttpClient>();
            var searchConfiguration = new Mock<IOptionsMonitor<SearchConfiguration>>();
            searchConfiguration.Setup(x => x.CurrentValue).Returns(new SearchConfiguration
            {
                DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
                DefaultKeyword = "e-settlements",
                DefaultUrl = "https://www.sympli.com.au",
                DefaultTotalItem = 100,
                DefaultUrlSearchGoogle = "Sample",
                DefaultUrlSearchBing = "Sample",
            });
            var urlSearch = CommonUtils.GetDefaulGoogleSearch(searchConfiguration.Object.CurrentValue.DefaultUrlSearchGoogle,
                request.Keywords, searchConfiguration.Object.CurrentValue.DefaultTotalItem);
            var searchService = new SearchService(httpClient.Object, searchConfiguration.Object);

            // Action
            var result = await searchService.SearchAsync(request);

            // Assert
            var errorMessage = string.Format(SympliTrackingMessage.Error.ErrorReadUrl, urlSearch);
            Assert.NotNull(result);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.True(string.IsNullOrEmpty(result.Data));
            Assert.True(result.Message == errorMessage);
        }

        [Fact]
        public async Task SearchAsync_Expect_NotFoundResultBlockSearch()
        {
            // Arrange
            var request = new GetSeoRequest
            {
                Keywords = "Sample",
                Url = "sympli.com.au",
                SearchEngine = Domains.Enums.SearchEngineEnum.Google
            };

            var httpClient = new Mock<HttpClient>();
            var searchConfiguration = new Mock<IOptionsMonitor<SearchConfiguration>>();
            searchConfiguration.Setup(x => x.CurrentValue).Returns(new SearchConfiguration
            {
                DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
                DefaultKeyword = "e-settlements",
                DefaultUrl = "https://www.sympli.com.au",
                DefaultTotalItem = 100,
                DefaultUrlSearchGoogle = "https://vnexpress.net/",
                DefaultUrlSearchBing = "Sample",
            });
            var urlSearch = CommonUtils.GetDefaulGoogleSearch(searchConfiguration.Object.CurrentValue.DefaultUrlSearchGoogle,
                request.Keywords, searchConfiguration.Object.CurrentValue.DefaultTotalItem);
            var searchService = new SearchService(httpClient.Object, searchConfiguration.Object);

            // Action
            var result = await searchService.SearchAsync(request);

            // Assert

            var errorMessage = string.Format(SympliTrackingMessage.Error.ErrorNotFoundResultUrl, urlSearch);
            Assert.NotNull(result);
            Assert.True(result.StatusCode == HttpStatusCode.NoContent);
            Assert.True(string.IsNullOrEmpty(result.Data));
            Assert.True(result.Message == errorMessage);
        }
    }
}