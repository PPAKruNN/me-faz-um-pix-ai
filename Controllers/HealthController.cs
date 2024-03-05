using Microsoft.AspNetCore.Mvc;

namespace Health.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Health() {
        return Ok("I'm Alive");
    }
}