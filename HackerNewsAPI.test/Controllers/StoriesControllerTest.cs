using HackerNewsAPI.Constant;
using HackerNewsAPI.Interfaces;
using HackerNewsAPI.Model.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HackerNewsAPI.Controllers.test
{
    public class StoriesControllerTest
    {
        private readonly Mock<IHackerNewsService> _mockHackerNewsService;
        private readonly IMemoryCache _mockCache;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly StoriesController _controller;

        public StoriesControllerTest()
        {
            _mockHackerNewsService = new Mock<IHackerNewsService>();
            _mockCache = new MemoryCache(new MemoryCacheOptions());
            _mockConfiguration = new Mock<IConfiguration>();

            _controller = new StoriesController(
                _mockHackerNewsService.Object,
                _mockCache,
                _mockConfiguration.Object
            );
        }

        [Fact]
        public async Task Get_ReturnsOk_WithStoriesFromCache()
        {
            // Arrange
            var stories = new List<Story> { new Story { Id = 1, Title = "Test Story", Type = "story" }, new Story { Id = 2, Title = "Test Story 2", Type = "story" } };
            _mockCache.Set(ApplicationConstant.storiesCacheKey, stories, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            });

            // Act
            var result = await _controller.Get(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStories = Assert.IsAssignableFrom<IEnumerable<Story>>(okResult.Value);
            Assert.NotNull(returnStories);
            Assert.Equal(1, returnStories.FirstOrDefault().Id);
        }

        [Fact]
        public async Task Get_ReturnsOk_WithStoriesFromService_WhenCacheMiss()
        {
            // Arrange
            var stories = new List<Story>();
            _mockCache.Set(ApplicationConstant.storiesCacheKey, stories, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            });

            // Act
            var result = await _controller.Get(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStories = Assert.IsAssignableFrom<IEnumerable<Story>>(okResult.Value);
            Assert.Equal(0, returnStories.Count());
        }
    }
}
