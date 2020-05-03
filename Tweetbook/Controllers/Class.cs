using Microsoft.AspNetCore.Mvc;

namespace Tweetbook.Controllers
{
    public class TestController : Controller
    {
        [HttpGet("api/user")]
        public IActionResult Get()
        {
            return Ok(new { name = "Nick" });
        }
    }
}
