using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    public ActionResult<TResult> OkOrNotFound<TResult>(TResult result)
        => result is null ? NotFound() : Ok(result);
}