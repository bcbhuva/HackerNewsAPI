namespace HackerNewsAPI.Repositories
{
    using Constant;
    using Interfaces;
    using Model.Response;
    using Newtonsoft.Json;
    using System.Threading;

    public class HackerNewsRepository : IHackerNewsRepository
    {
        private readonly HttpClient _httpClient;

        public HackerNewsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get StoryIds
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<int>> GetStoryIdsAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"{ApplicationConstant.HACKERNEWS_BASEURL}/topstories.json");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<int>>(responseContent);
            }
            else
            {
                return new List<int>();
            }
        }

        /// <summary>
        /// Get Story By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"{ApplicationConstant.HACKERNEWS_BASEURL}/item/{id}.json");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Story>(responseContent);
            }
            return null;
        }
    }
}
