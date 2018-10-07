using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_middleware.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class HomeController:ControllerBase
    {
        [HttpGet]
        public IActionResult GetAction()
        {
            return Ok("Hello world!");
        }
    }
}