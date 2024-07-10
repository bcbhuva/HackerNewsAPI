namespace HackerNews.Services.Interfaces
{
    using Model.Response;

    public interface IHackerNewsService
    {
        Task<IEnumerable<Story>> GetStoriesAsync(CancellationToken cancellationToken);
    }
}
