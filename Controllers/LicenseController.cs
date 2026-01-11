using System.Net;
using LicenseService.Model;
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
        public IActionResult GenerateDemoLicenseAsync([FromBody] string fingerPrint)
        {
            var payload = service.CreateLicenseDemoAsync(fingerPrint);
            return Ok(
                new BaseDto(HttpStatusCode.OK, payload, Guid.NewGuid(), "Demo license generation successful", DateTime.UtcNow)
            );
        }

        [HttpPost("generate")]
        public IActionResult GenerateStandardLicenseAsync()
        {
            return Ok(new { message = "Standard license generated" });
        }
    }
}
