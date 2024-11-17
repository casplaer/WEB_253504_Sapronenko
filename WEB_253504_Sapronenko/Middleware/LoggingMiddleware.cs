namespace WEB_253504_Sapronenko.UI.Middleware
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

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

            await _next(context);

            var statusCode = context.Response.StatusCode;

            if (statusCode != 200)
            {
                _logger.LogError($"Bad response: {context.Request.Method} {context.Request.Path} responded with {context.Response.StatusCode}");
            }
            else
            {
                _logger.LogInformation($"Response: {context.Request.Method} {context.Request.Path} responded with {context.Response.StatusCode}");
            }
        }
    }
}
