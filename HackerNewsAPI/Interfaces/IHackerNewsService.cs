namespace HackerNewsAPI.Interfaces
{
    using HackerNewsAPI.Model.Response;

    public interface IHackerNewsService
    {
        Task<IEnumerable<Story>> GetStoriesAsync(CancellationToken cancellationToken);
    }
}
