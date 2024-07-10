namespace HackerNews.Services.Services
{
    using HackerNews.Services.Constant;
    using Interfaces;
    using Microsoft.Extensions.Caching.Memory;
    using Model.Response;
    using System.Threading;

    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHackerNewsRepository _repository;
        private IMemoryCache _cache;

        public HackerNewsService(IHackerNewsRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Get Stories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Story>> GetStoriesAsync(CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(ApplicationConstant.storiesCacheKey, out IEnumerable<Story> validStories))
            {
                //stories not found in cache. Fetching from external service.
                var storyIds = await _repository.GetStoryIdsAsync(cancellationToken);

                var storyTasks = storyIds.Select(id => _repository.GetStoryByIdAsync(id, cancellationToken));

                var stories = await Task.WhenAll(storyTasks);

                validStories = stories
                   .Where(story => story != null && story.Type.Equals("story", StringComparison.OrdinalIgnoreCase) && story.Url != null)
                   .Take(200)
                   .ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(ApplicationConstant.CacheDuration))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(ApplicationConstant.CacheDuration))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                _cache.Set(ApplicationConstant.storiesCacheKey, stories, cacheEntryOptions);

            }

            return validStories;
        }
    }
}
