using System.Net;
using LicenseService.Model;
using LicenseService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LicenseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("exchange")]
        public async Task<IActionResult> ExchangeAsync([FromBody] ExchangeRequest request)
        {
            var res = await service.ExchangeAsync(request);
            return Ok(new BaseDto(
                HttpStatusCode.OK,
                res,
                Guid.NewGuid(),
                "Exchange successful",
                DateTime.UtcNow.ToLocalTime()
            ));
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyAsync([FromBody] VerifyRequest request)
        {
            var res = await service.VerifyAsync(request);
            return Ok(
                new BaseDto(
                    HttpStatusCode.OK,
                    res,
                    Guid.NewGuid(),
                    "Verification completed",
                    DateTime.UtcNow.ToLocalTime()
                )
            );
        }
    }
}
