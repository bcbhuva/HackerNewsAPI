using HackerNewsAPI.Model.Response;
using Newtonsoft.Json;
using System.Net;

namespace HackerNewsAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, 0, ex.Message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, int errorCode, string errorMessage)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponse();
            if (errorCode != 0)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.ErrorCode = errorCode;
                errorResponse.ErrorMessage = errorMessage;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.ErrorMessage = "Internal server error!";
            }

            var result = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
