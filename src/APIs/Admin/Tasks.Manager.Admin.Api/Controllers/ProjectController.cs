using Microsoft.AspNetCore.Mvc;

namespace Tasks.Manager.Admin.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ProjectController : ControllerBase
{
    [HttpGet("test-route")]
    public IActionResult TestRoute()
    {
        return Ok("A rota está funcionando!");
    }
}
