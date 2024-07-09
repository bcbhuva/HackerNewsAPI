namespace HackerNewsAPI.Model.Response
{
    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
