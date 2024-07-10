using HackerNewsAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.test.Controllers
{
    public class BaseControllerTest
    {
        private class TestController : BaseController { }

        [Fact]
        public void OkOrNotFound_ReturnsOk_WhenResultIsNotNull()
        {
            // Arrange
            var controller = new TestController();
            var testResult = new { Id = 1, Name = "Test" };

            // Act
            var result = controller.OkOrNotFound(testResult);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void OkOrNotFound_ReturnsNotFound_WhenResultIsNull()
        {
            // Arrange
            var controller = new TestController();
            object testResult = null;

            // Act
            var result = controller.OkOrNotFound(testResult);

            // Assert
            var actionResult = Assert.IsType<ActionResult<object>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
