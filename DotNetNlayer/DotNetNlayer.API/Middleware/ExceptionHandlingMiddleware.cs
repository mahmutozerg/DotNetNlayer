using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private const string UnexpectedErrorMessage = "Unexpected error";

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var messages = new List<string>();
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var title = "Server error";
            
            switch (exception)
            {
                case AlreadyExistException alreadyExistException:
                {
                    title = alreadyExistException.Title;
                    statusCode = (int)alreadyExistException.StatusCode;
                    
                    var formattedMessage = FormatExceptionMessage(alreadyExistException.MessageFormat, alreadyExistException.MessageProps);

                    messages.Add(formattedMessage);

                    break;
                 }
              case CustomBaseException customBaseException:
                {
                    statusCode = (int)customBaseException.StatusCode;
                    title = customBaseException.Title;
                    
                    var formattedMessage = FormatExceptionMessage(customBaseException.MessageFormat, customBaseException.MessageProps);
                    messages.Add(formattedMessage);
                    break;
                }
                default:
                    messages.Add(UnexpectedErrorMessage);
                    break;
            }

            var problemDetails = new ProblemDetails
            { 
                Status = statusCode,
                Title = title,
                Detail = string.Join(Environment.NewLine,messages.ToArray())
            };
            
            logger.LogError(exception,"Exception Detail {@ExceptionStatus} - {@ExceptionTitle} - {@ExceptionDetail} - {@ContextPath}",
                problemDetails.Status,
                problemDetails.Title,
                problemDetails.Detail,
                context.Request.Path.Value);

            
            
            var responseResult = JsonSerializer.Serialize(CustomResponseDto<ProblemDetails>.Exception(problemDetails));
            await context.Response.WriteAsync(responseResult);
        }
    }
    /// <summary>
    ///  Helper method for formatting the exception
    /// </summary>
    /// <param name="messageFormat"></param>
    /// <param name="messageProps"></param>
    /// <returns></returns>
    private static string FormatExceptionMessage(string messageFormat, Dictionary<string, string> messageProps)
    {
        foreach (var (key, value) in messageProps)
        {
            messageFormat = messageFormat.Replace(key, value);
        }
        return messageFormat;
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}