using System.Net;
using System.Text.Json;
using Framework.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Framework.Core.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;
    
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
            
            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status401Unauthorized:
                {
                    var response = new ApiResponse(401);
                    await WriteResponse(context, response);
                    break;
                }
            }
        }
        catch (AppException ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode != 0 ? ex.StatusCode : (int) HttpStatusCode.InternalServerError;

            var response = new ApiResponse(ex.StatusCode, ex.Message);
            
            var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace!)
                : new ApiException(context.Response.StatusCode, "Internal Server Error");

            var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }

    private async Task WriteResponse(HttpContext context, ApiResponse response)
    {
        var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }
    
}