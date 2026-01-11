using LicenseService.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LicenseService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicenseController(ILicenseService service) : ControllerBase
    {
        [HttpPost("generate/demo")]
        public IActionResult GenerateDemoLicenseAsync()
        {
            return Ok(new { message = "Demo license generated" });
        }

        [HttpPost("generate")]
        public IActionResult GenerateStandardLicenseAsync()
        {
            return Ok(new { message = "Standard license generated" });
        }
    }
}
