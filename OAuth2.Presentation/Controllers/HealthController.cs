using Microsoft.AspNetCore.Mvc;

namespace OAuth2.Presentation.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("OK");
    }
}
