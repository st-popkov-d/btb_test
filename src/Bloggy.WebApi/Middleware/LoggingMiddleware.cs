namespace Bloggy.WebApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("[REQ] [{Method}] [{Timestamp}] {Path}", context.Request.Method, DateTimeOffset.UtcNow, context.Request.Path);
            await _next(context);
            // This naive approach will not properly handle all scenarios, i.e. IAsyncEnumerable returns of Stream returns
            _logger.LogInformation("[RES] [{Method}] [{Status}] [{Timestamp}] {Path}", context.Request.Method, context.Response.StatusCode, DateTimeOffset.UtcNow, context.Request.Path);
        }
    }
}
