using HackerNews.Services.Interfaces;
using HackerNews.Services.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers
{
    public class StoriesController : BaseController
    {
        private readonly IHackerNewsService _hackerNewsService;

        public StoriesController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
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
            var stories = await _hackerNewsService.GetStoriesAsync(cancellationToken);

            if (stories == null)
            {
                return NoContent();
            }

            return Ok(stories);
        }
    }
}
