using HackerNews.Services.Interfaces;
using HackerNews.Services.Repositories;
using HackerNews.Services.Services;

namespace HackerNewsAPI.Ioc
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Register services to the dependency injection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns></returns>
        public static void AddOperationProvider(this IServiceCollection services)
        {
            services.AddHttpClient<IHackerNewsRepository, HackerNewsRepository>();
            services.AddScoped<IHackerNewsService, HackerNewsService>();
        }
    }
}
