namespace Arksoft.CustomerManagement.Web.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private const string ApiKeyHeaderName = "X-API-KEY";
        private const string ValidApiKey = "arksoft-demo-key-123456";

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only check API key for /api routes
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            var apiKey = context.Request.Headers[ApiKeyHeaderName].FirstOrDefault();

            if (apiKey != ValidApiKey)
            {
                _logger.LogWarning("Invalid or missing API key from {RemoteIp}", 
                    context.Connection.RemoteIpAddress);
                    
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized - Invalid API Key");
                return;
            }

            _logger.LogInformation("Valid API request from {RemoteIp}", 
                context.Connection.RemoteIpAddress);

            await _next(context);
        }
    }
}