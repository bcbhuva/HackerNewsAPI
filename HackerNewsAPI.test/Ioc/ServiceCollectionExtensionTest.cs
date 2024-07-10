using HackerNews.Services.Interfaces;
using HackerNewsAPI.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNewsAPI.test.Ioc
{
    public class ServiceCollectionExtensionTest
    {
        [Fact]
        public void AddOperationProvider_ShouldRegisterIHackerNewsRepository_AsHttpClient()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddOperationProvider();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.NotNull(httpClientFactory);

            var hackerNewsRepository = serviceProvider.GetService<IHackerNewsRepository>();
            Assert.NotNull(hackerNewsRepository);
        }
    }
}
