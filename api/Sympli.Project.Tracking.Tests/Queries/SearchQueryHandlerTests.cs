using Microsoft.Extensions.Caching.Memory;
using Moq;
using Sympli.Project.Tracking.Applications.Interfaces;
using Sympli.Project.Tracking.Applications.QueryHandlers;
using Sympli.Project.Tracking.Domains.Requests;
using Sympli.Project.Tracking.Domains.Responses;
using Sympli.Project.Tracking.Infrastructures.Cache.Interfaces;
using System.Net;

namespace Sympli.Project.Tracking.Tests.Queries
{
    public class SearchQueryHandlerTests : BaseUnitTest
    {
        [Fact]
        public async Task SearchQueryHandler_Expect_SuccessWithNoCachedFound()
        {
            // Arrange
            var request = new GetSeoRequest
            {
                Keywords = "e-settlements",
                Url = "https://www.sympli.com.au",
                SearchEngine = Domains.Enums.SearchEngineEnum.Google
            };

            var searchService = new Mock<ISearchService>();
            var cacheService = new Mock<ICacheService>();

            cacheService.Setup(x
                => x.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(false);

            searchService.Setup(x
                => x.SearchAsync(It.IsAny<GetSeoRequest>()))
                .Returns(Task.FromResult(new Response<string>
                {
                    Data = "Sample",
                    StatusCode = HttpStatusCode.OK
                }));

            cacheService.Setup(x
                => x.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MemoryCacheEntryOptions>()));

            var getSeoHandler = new GetSeoHandler(searchService.Object, cacheService.Object, SearchConfiguration);

            // Action
            var result = await getSeoHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(!string.IsNullOrEmpty(result.Data));
            cacheService.Verify(m => m.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny), Times.Once);
            cacheService.Verify(m => m.Set(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
        }

        [Fact]
        public async Task SearchQueryHandler_Expect_SuccessWithCachedMemory()
        {
            // Arrange
            var request = new GetSeoRequest
            {
                Keywords = "e-settlements",
                Url = "https://www.sympli.com.au",
                SearchEngine = Domains.Enums.SearchEngineEnum.Google
            };

            var searchService = new Mock<ISearchService>();
            var cacheService = new Mock<ICacheService>();

            cacheService.Setup(x
                => x.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns((object key, out string value) => { value = "Sample"; return true; });

            var getSeoHandler = new GetSeoHandler(searchService.Object, cacheService.Object, SearchConfiguration);

            // Action
            var result = await getSeoHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(!string.IsNullOrEmpty(result.Data));
            cacheService.Verify(m => m.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny), Times.Once);
            cacheService.Verify(m => m.Set(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Never);
        }

        [Fact]
        public async Task SearchQueryHandler_Expect_SearchFailure()
        {
            // Arrange
            var request = new GetSeoRequest
            {
                Keywords = "e-settlements",
                Url = "https://www.sympli.com.au",
                SearchEngine = Domains.Enums.SearchEngineEnum.Google
            };

            var searchService = new Mock<ISearchService>();
            var cacheService = new Mock<ICacheService>();

            cacheService.Setup(x
                => x.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(false);

            searchService.Setup(x
                => x.SearchAsync(It.IsAny<GetSeoRequest>()))
                .Returns(Task.FromResult(new Response<string>
                {
                    Data = string.Empty,
                    StatusCode = HttpStatusCode.NotFound
                }));

            var getSeoHandler = new GetSeoHandler(searchService.Object, cacheService.Object, SearchConfiguration);

            // Action
            var result = await getSeoHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.True(string.IsNullOrEmpty(result.Data));
            cacheService.Verify(m => m.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny), Times.Once);
            cacheService.Verify(m => m.Set(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Never);
        }
    }
}