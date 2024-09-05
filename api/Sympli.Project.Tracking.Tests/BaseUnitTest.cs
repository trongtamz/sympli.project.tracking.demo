using Microsoft.Extensions.Options;
using Moq;
using Sympli.Project.Tracking.Domains.Configurations;

namespace Sympli.Project.Tracking.Tests
{
    public class BaseUnitTest
    {
        protected readonly IOptionsMonitor<SearchConfiguration> SearchConfiguration;

        public BaseUnitTest()
        {
            var mockSearchConfiguration = new Mock<IOptionsMonitor<SearchConfiguration>>();
            mockSearchConfiguration.Setup(x => x.CurrentValue).Returns(new SearchConfiguration
            {
                DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
                DefaultKeyword = "e-settlements",
                DefaultUrl = "https://www.sympli.com.au",
                DefaultTotalItem = 100,
                DefaultUrlSearchGoogle = "https://www.google.com.au",
                DefaultUrlSearchBing = "https://www.bing.com"
            });
            SearchConfiguration = mockSearchConfiguration.Object;
        }
    }
}