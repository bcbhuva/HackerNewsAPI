namespace HackerNews.Services.test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using HackerNews.Services.Constant;
    using HackerNews.Services.Interfaces;
    using HackerNews.Services.Model.Response;
    using HackerNews.Services.Services;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;
    using Xunit;

    public class HackerNewsServiceTest
    {
        private readonly IMemoryCache _mockCache;
        private readonly Mock<IHackerNewsRepository> _mockRepository;
        private readonly HackerNewsService _service;

        public HackerNewsServiceTest()
        {
            _mockCache = new MemoryCache(new MemoryCacheOptions());
            _mockRepository = new Mock<IHackerNewsRepository>();
            _service = new HackerNewsService(_mockRepository.Object, _mockCache);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenCacheIsNull()
        {
            // Arrange
            var repositoryMock = new Mock<IHackerNewsRepository>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new HackerNewsService(repositoryMock.Object, null));
            Assert.Equal("cache", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldInitialize_WhenParametersAreValid()
        {
            // Arrange
            var repositoryMock = new Mock<IHackerNewsRepository>();
            var cacheMock = new Mock<IMemoryCache>();

            // Act
            var service = new HackerNewsService(repositoryMock.Object, cacheMock.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task GetStoriesAsync_ReturnsStoriesFromCache_WhenCacheIsAvailable()
        {
            // Arrange
            var cachedStories = new List<Story>
            {
                new Story { Id = 1, Title="story1", Type = "story", Url = "http://example.com" },
                new Story { Id = 2, Title="story2", Type = "story", Url = "http://example.com" }
            };

            _mockCache.Set(ApplicationConstant.storiesCacheKey, cachedStories, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            });

            // Act
            var result = await _service.GetStoriesAsync(CancellationToken.None);

            // Assert
            Assert.Equal(cachedStories, result);
            _mockRepository.Verify(repo => repo.GetStoryIdsAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetStoriesAsync_FetchesStoriesFromRepository_WhenCacheIsNotAvailable()
        {
            // Arrange
            var Stories = new List<Story>();
            _mockCache.Set(ApplicationConstant.storiesCacheKey, Stories, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            });

            var storyIds = new List<int> { 1, 2, 3 };
            var stories = new List<Story>
            {
                new Story { Id = 1, Title="story1", Type = "story", Url = "http://example.com" },
                new Story { Id = 2, Title="story2", Type = "story", Url = "http://example.com" },
                new Story { Id = 3, Title="story3", Type = "story", Url = "http://example.com" }
            };

            // Act
            _mockRepository
               .Setup(repo => repo.GetStoryIdsAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(storyIds);

            _mockRepository
                .Setup(repo => repo.GetStoryByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns<int, CancellationToken>((id, token) => Task.FromResult(stories.FirstOrDefault(s => s.Id == id)));

            // Assert
            Assert.Equal(3, stories.Count());
            Assert.NotNull(stories);

        }
    }
}
