using HackerNewsAPI.Constant;
using HackerNewsAPI.Interfaces;
using HackerNewsAPI.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Controllers
{
    public class StoriesController : BaseController
    {
        private readonly IHackerNewsService _hackerNewsService;
        private IMemoryCache _cache;
        private IConfiguration _configuration;

        public StoriesController(IHackerNewsService hackerNewsService, IMemoryCache cache, IConfiguration configuration)
        {
            _hackerNewsService = hackerNewsService;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _configuration = configuration ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Get Latest Stories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Story))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(ApplicationConstant.storiesCacheKey, out IEnumerable<Story> stories))
            {
                //stories not found in cache. Fetching from database or external service.
                stories = await _hackerNewsService.GetStoriesAsync(cancellationToken);

                Double CachingDuration = Convert.ToDouble(_configuration.GetValue<string>("CachingDuration"));

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(CachingDuration))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(CachingDuration))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                _cache.Set(ApplicationConstant.storiesCacheKey, stories, cacheEntryOptions);

            }

            return Ok(stories);
        }
    }
}
