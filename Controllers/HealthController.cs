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
            return Ok(new BaseDto<HealthDto>(
                System.Net.HttpStatusCode.OK,
                new HealthDto("UP", DateTime.UtcNow),
                Guid.NewGuid(),
                "Service is UP",
                DateTime.UtcNow
            ));
        }
    }
}
