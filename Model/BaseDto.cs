using System;
using System.Net;

namespace LicenseService.Model;

public record BaseDto<T>(HttpStatusCode code, T payload, Guid uuid, string message, DateTime timeStamp);
