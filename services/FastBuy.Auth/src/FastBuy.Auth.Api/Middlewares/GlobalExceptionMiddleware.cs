using System.Text.Json;

namespace FastBuy.Auth.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error has occurred - {ex.Message}");


            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var error = new
            {
                TimeStamp = DateTime.UtcNow,
                Path = context.Request.Path.Value,
                Status = context.Response.StatusCode,
                Description = context.Response.StatusCode != 500
                              ? ex.Message
                              : "An unexpected error has occurred"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
}
