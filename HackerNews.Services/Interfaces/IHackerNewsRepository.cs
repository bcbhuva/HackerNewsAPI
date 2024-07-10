namespace HackerNews.Services.Interfaces
{
    using Model.Response;

    public interface IHackerNewsRepository
    {
        Task<List<int>> GetStoryIdsAsync(CancellationToken cancellationToken);
        Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken);
    }
}
