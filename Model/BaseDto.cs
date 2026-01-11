using System;
using System.Net;

namespace LicenseService.Model;

public record BaseDto(HttpStatusCode code, object payload, Guid uuid, string message, DateTime timeStamp);
