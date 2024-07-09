namespace HackerNewsAPI.Model.Response
{
    public class Story
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Type { get; set; }
        public string? Url { get; set; }
    }
}
