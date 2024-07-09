namespace HackerNewsAPI.Interfaces
{
    using HackerNewsAPI.Model.Response;

    public interface IHackerNewsRepository
    {
        Task<List<int>> GetStoryIdsAsync(CancellationToken cancellationToken);
        Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken);
    }
}
