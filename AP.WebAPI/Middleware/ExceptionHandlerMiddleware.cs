using System.Net;
using AP.BusinessInterfaces.Data.Error;

namespace AP.WebAPI.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occured");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var response = new APWebAPIError
            {
                Message = ex.Message,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}