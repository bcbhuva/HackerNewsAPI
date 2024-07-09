using HackerNewsAPI.Interfaces;
using HackerNewsAPI.Model.Response;
using HackerNewsAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNewsAPI.test
{
    public class HackerNewsServicesTest
    {
        private readonly Mock<IHackerNewsRepository> _mockRepository;
        private readonly HackerNewsService _hackerNewsService;

        public HackerNewsServicesTest()
        {
            _mockRepository = new Mock<IHackerNewsRepository>();
            _hackerNewsService = new HackerNewsService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetStoriesAsync_ShouldReturnStories_WhenRepositoryReturnsValidData()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var storyIds = Enumerable.Range(1, 220).ToList();
            var stories = storyIds.Select(id => new Story
            {
                Id = id,
                Type = "story",
                Url = $"http://example.com/story/{id}",
                Title = "Title",
            }).ToList();

            _mockRepository.Setup(repo => repo.GetStoryIdsAsync(cancellationToken)).ReturnsAsync(storyIds);
            _mockRepository.Setup(repo => repo.GetStoryByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync((int id, CancellationToken ct) => stories.FirstOrDefault(s => s.Id == id));

            // Act
            var result = await _hackerNewsService.GetStoriesAsync(cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Count());
        }

        [Fact]
        public async Task GetStoriesAsync_ShouldReturnEmptyList_WhenRepositoryReturnsEmptyStoryIds()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var storyIds = new List<int>();

            _mockRepository.Setup(repo => repo.GetStoryIdsAsync(cancellationToken)).ReturnsAsync(storyIds);

            // Act
            var result = await _hackerNewsService.GetStoriesAsync(cancellationToken);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetStoriesAsync_ShouldReturnOnlyStoryTypeItems_WhenRepositoryReturnsMultipleTypes()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var storyIds = new List<int> { 1, 2, 3 };
            var stories = new List<Story>
        {
            new Story { Id = 1, Title="story1", Type = "story", Url = "http://example.com/story/1" },
            new Story { Id = 2, Title="story2", Type = "comment", Url = "http://example.com/comment/2" },
            new Story { Id = 3, Title="story3", Type = "story", Url = "http://example.com/story/3" }
        };

            _mockRepository.Setup(repo => repo.GetStoryIdsAsync(cancellationToken)).ReturnsAsync(storyIds);
            _mockRepository.Setup(repo => repo.GetStoryByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync((int id, CancellationToken ct) => stories.FirstOrDefault(s => s.Id == id));

            // Act
            var result = await _hackerNewsService.GetStoriesAsync(cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
