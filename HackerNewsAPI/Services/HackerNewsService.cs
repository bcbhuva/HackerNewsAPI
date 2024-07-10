namespace HackerNewsAPI.Services
{
    using Interfaces;
    using Model.Response;
    using System.Threading;

    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHackerNewsRepository _repository;

        public HackerNewsService(IHackerNewsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get Stories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Story>> GetStoriesAsync(CancellationToken cancellationToken)
        {
            var storyIds = await _repository.GetStoryIdsAsync(cancellationToken);

            var stories = new List<Story>();
            foreach (var id in storyIds)
            {
                var story = await _repository.GetStoryByIdAsync(id, cancellationToken);

                if (story != null && story.Type.ToLower() == "story" && story.Url != null)
                    stories.Add(story);
            }
            return stories.Take(200);
        }
    }
}
