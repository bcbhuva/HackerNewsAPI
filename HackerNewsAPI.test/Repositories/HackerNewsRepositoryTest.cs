using HackerNewsAPI.Model.Response;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace HackerNewsAPI.Repositories.test
{
    public class HackerNewsRepositoryTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly HackerNewsRepository _hackerNewsRepository;

        public HackerNewsRepositoryTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/")
            };
            _hackerNewsRepository = new HackerNewsRepository(_httpClient);
        }

        [Fact]
        public async Task GetStoryIdsAsync_ReturnsStoryIds_WhenResponseIsSuccess()
        {
            // Arrange
            var storyIds = new List<int> { 1, 2, 3, 4, 5 };
            var responseContent = JsonConvert.SerializeObject(storyIds);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _hackerNewsRepository.GetStoryIdsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(storyIds.Count, result.Count);
            Assert.Equal(storyIds, result);
        }

        [Fact]
        public async Task GetStoryIdsAsync_ReturnsEmptyList_WhenResponseIsNotSuccess()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _hackerNewsRepository.GetStoryIdsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetStoryByIdAsync_ReturnsStory_WhenResponseIsSuccess()
        {
            // Arrange
            var storyId = 1;
            var story = new Story
            {
                Id = storyId,
                Title = "Test Story",
                Url = "http://teststory.com",
                Type ="story"
            };
            var responseContent = JsonConvert.SerializeObject(story);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _hackerNewsRepository.GetStoryByIdAsync(storyId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(storyId, result.Id);
            Assert.Equal(story.Title, result.Title);
            Assert.Equal(story.Url, result.Url);
        }

        [Fact]
        public async Task GetStoryByIdAsync_ReturnsNull_WhenResponseIsNotSuccess()
        {
            // Arrange
            var storyId = 1;
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _hackerNewsRepository.GetStoryByIdAsync(storyId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
