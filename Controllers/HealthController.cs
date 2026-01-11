using LicenseService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LicenseService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Health : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult GetHealthAsync()
        {
            return Ok(new BaseDto(
                System.Net.HttpStatusCode.OK,
                new { status = "UP" },
                Guid.NewGuid(),
                "Service is UP",
                DateTime.UtcNow
            ));
        }
    }
}
