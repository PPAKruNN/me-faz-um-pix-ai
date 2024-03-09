using System.Net;
using FazUmPix.Exceptions;
using FazUmPix.Models;

namespace FazUmPix.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
  private readonly RequestDelegate _next = next;
  private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    _logger.LogError(exception, "An unexpected error occurred.");

    ExceptionResponse response = exception switch
    {
      PixKeyNotFoundException => new ExceptionResponse(HttpStatusCode.NotFound, exception.Message),
      KeyAlreadyExistsException => new ExceptionResponse(HttpStatusCode.Conflict, exception.Message),
      ReachedKeyLimitException => new ExceptionResponse(HttpStatusCode.Forbidden, exception.Message),
      NoPermissionToModifyAnotherUserAccountException => new ExceptionResponse(HttpStatusCode.Unauthorized, exception.Message),
      UserNotFoundException => new ExceptionResponse(HttpStatusCode.NotFound, exception.Message),
      InvalidPaymentProviderException => new ExceptionResponse(HttpStatusCode.UnprocessableEntity, exception.Message),
      InvalidCpfException => new ExceptionResponse(HttpStatusCode.UnprocessableEntity, exception.Message),
      InvalidKeyFormatException => new ExceptionResponse(HttpStatusCode.UnprocessableEntity, exception.Message),
      

      _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
    };

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)response.StatusCode;
    await context.Response.WriteAsJsonAsync(response);
  }
}

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);