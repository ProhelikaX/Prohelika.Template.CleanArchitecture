namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Middlewares;

/// <summary>
/// Middleware for logging
/// </summary>
public class LoggerMiddleware(ILoggerFactory loggerFactory) : IMiddleware
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<LoggerMiddleware>();

    /// <summary>
    /// Invokes the middleware
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        finally
        {
            _logger.LogInformation("Request {RequestMethod} {PathValue} => {ResponseStatusCode}",
                context.Request.Method, context.Request.Path.Value, context.Response.StatusCode);
        }
    }
}