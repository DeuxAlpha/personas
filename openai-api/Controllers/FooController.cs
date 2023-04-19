using Microsoft.AspNetCore.Mvc;

namespace openai_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FooController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}