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
        public async Task<IActionResult> GenerateDemoLicenseAsync([FromBody] GenerateDemo fingerPrint)
        {
            var payload = await service.CreateLicenseDemoAsync(fingerPrint);
            return Ok(
                new BaseDto(HttpStatusCode.OK, payload, Guid.NewGuid(), payload is null ? "Demo license generation failed" : "Demo license generation successful", DateTime.UtcNow)
            );
        }

        [HttpPost("generate")]
        public Task<IActionResult> GenerateStandardLicenseAsync()
        {
            return Task.FromResult<IActionResult>(Ok(new { message = "Standard license generated" }));
        }
    }
}
