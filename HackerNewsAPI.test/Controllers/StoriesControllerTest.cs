using HackerNews.Services.Interfaces;
using HackerNews.Services.Model.Response;
using HackerNewsAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HackerNewsAPI.test.Controllers
{
    public class StoriesControllerTest
    {
        private readonly Mock<IHackerNewsService> _mockHackerNewsService;
        private readonly StoriesController _controller;

        public StoriesControllerTest()
        {
            _mockHackerNewsService = new Mock<IHackerNewsService>();

            _controller = new StoriesController(_mockHackerNewsService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithStories()
        {
            // Arrange
            List<Story> stories = new List<Story>
            {
                new Story { Id = 1, Title="story1", Type = "story", Url = "http://example.com" },
                new Story { Id = 2, Title="story2", Type = "story", Url = "http://example.com" }
            };

            _mockHackerNewsService
                .Setup(service => service.GetStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stories);

            // Act
            var result = await _controller.Get(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStories = Assert.IsAssignableFrom<IEnumerable<Story>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(2, returnStories.Count());
        }

        [Fact]
        public async Task Get_ReturnsNoContentResult_WhenNoStories()
        {
            // Arrange
            _mockHackerNewsService
                .Setup(service => service.GetStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Story>)null);

            // Act
            var result = await _controller.Get(CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsNoContentResult_WhenEmptyStories()
        {
            // Arrange
            var stories = new List<Story>();

            _mockHackerNewsService
                .Setup(service => service.GetStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stories);

            // Act
            var result = await _controller.Get(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStories = Assert.IsAssignableFrom<IEnumerable<Story>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Empty(returnStories);
        }
    }
}
