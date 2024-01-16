using System.Net;
using Microsoft.AspNetCore.Mvc;
using Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;
using Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Extensions;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Middlewares;

/// <summary>
/// Middleware to handle exceptions
/// </summary>
public class ExceptionMiddleware(ILoggerFactory loggerFactory) : IMiddleware
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling request: {RequestPath}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        ProblemDetails problemDetails = new()
        {
            Instance = context.Request.Path,
            Detail = exception.Message
        };

        switch (exception)
        {
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                problemDetails.Title = "Not Found";
                problemDetails.Status = (int)HttpStatusCode.NotFound;
                break;
            case BadRequestException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                break;
            case ForbiddenException:
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                problemDetails.Title = "Forbidden";
                problemDetails.Status = (int)HttpStatusCode.Forbidden;
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "Internal Server Error";
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                break;
        }


        await context.Response.WriteAsync(problemDetails.ToJson());
    }
}