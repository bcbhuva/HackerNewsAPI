using HackerNewsAPI.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
namespace HackerNewsAPI.test.Middleware
{
    public class ExceptionHandlingMiddlewareTest
    {
        [Fact]
        public async Task InvokeAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            var requestDelegateMock = new Mock<RequestDelegate>();
            requestDelegateMock.Setup(rd => rd(It.IsAny<HttpContext>())).Throws(new Exception("Test exception"));

            var middleware = new ExceptionHandlingMiddleware(requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);
        }

        [Fact]
        public async Task InvokeAsync_ShouldCallNextDelegate_WhenNoExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            var requestDelegateMock = new Mock<RequestDelegate>();
            requestDelegateMock.Setup(rd => rd(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            var middleware = new ExceptionHandlingMiddleware(requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            requestDelegateMock.Verify(rd => rd(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task HandleExceptionAsync_ShouldSetResponseCorrectly()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var exception = new Exception("Test exception");

            // Act
            await ExceptionHandlingMiddleware.HandleExceptionAsync(context, exception);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);           
        }
    }
}
