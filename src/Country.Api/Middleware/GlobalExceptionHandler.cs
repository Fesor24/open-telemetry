using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Country.Api.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Message: {Message}", ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problemDetails = new ProblemDetails
                {
                    Detail = ex.Message,
                    Type = "internal server error",
                    Status = (int)HttpStatusCode.InternalServerError
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var content = JsonSerializer.Serialize(problemDetails, jsonOptions);

                await context.Response.WriteAsync(content);
            }
        }
    }
}
