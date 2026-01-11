using System;
using System.Net;
using System.Text.Json;
using LicenseService.Model;

namespace LicenseService.Exceptions;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
  public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
  {

    try
    {
      await next(httpContext);

    }
    catch (Exception ex)
    {
      await HandleException(httpContext, ex);
    }
  }


  private Task HandleException(HttpContext context, Exception ex)
  {

    context.Response.StatusCode = ex is TimeoutException ? (int)HttpStatusCode.RequestTimeout : (int)HttpStatusCode.InternalServerError;
    context.Response.ContentType = "application/json";

    return context.Response.WriteAsync(JsonSerializer.Serialize(new BaseDto(
      HttpStatusCode.InternalServerError,
      new List<string> { ex.Message },
      Guid.NewGuid(),
      "Internal Server Error",
      DateTime.UtcNow
    )));
  }
}