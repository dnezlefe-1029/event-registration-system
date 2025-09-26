using EventReg.Application.Common;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace EventReg.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ErrorHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _logger = logger;
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                context.Request?.Method,
                context.Request?.Path.Value);

            var (statusCode, message, errors) = MapException(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = ApiResponse<object>.Failed(message, statusCode, errors);

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private (int StatusCode, string Message, List<ApiError> Errors) MapException(Exception ex)
    {
        return ex switch
        {
            NotFoundException nf => ((int)HttpStatusCode.NotFound, nf.Message,
                new List<ApiError> { new() { Field = "Resource", Error = nf.Message } }
            ),

            UnauthorizedException un => ((int)HttpStatusCode.Unauthorized, un.Message,
                new List<ApiError> { new() { Field = "Auth", Error = un.Message } }
            ),

            ForbiddenException fb => ((int)HttpStatusCode.Forbidden, fb.Message,
                new List<ApiError> { new() { Field = "Permission", Error = fb.Message } }
            ),

            BadRequestException br => ((int)HttpStatusCode.BadRequest, br.Message,
                new List<ApiError> { new() { Field = "Request", Error = br.Message } }
            ),

            _ => ((int)HttpStatusCode.InternalServerError,
                "An unexpected error occured. Please try again later.",
                new List<ApiError>
                {
                    new()
                    {
                        Field = "Server",
                        Error = _env.IsDevelopment() ? ex.Message : "An unexpected error occured."
                    }
                })
        };
    }
}
