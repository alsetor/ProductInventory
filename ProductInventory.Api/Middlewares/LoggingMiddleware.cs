using System.Text;

namespace ProductInventory.Api.Middlewares
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
            context.Request.EnableBuffering();

            var requestBody = await ReadRequestBody(context);
            _logger.LogInformation("HTTP Request: {method} {url} \nQuery: {query} \nBody: {body}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                requestBody);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("HTTP Response: {statusCode} \nBody: {body}",
                context.Response.StatusCode,
                responseText);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> ReadRequestBody(HttpContext context)
        {
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }
    }
}