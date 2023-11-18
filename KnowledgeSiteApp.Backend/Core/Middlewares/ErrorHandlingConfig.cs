using KnowledgeSiteApp.Models.ErrorHandling;
using Microsoft.AspNetCore.Http;

namespace KnowledgeSiteApp.Backend.Core.ErrorHandlingMiddleware
{
    public class ErrorHandlingConfig
    {
        private readonly RequestDelegate next;
        private const string ApiKey = "X-API-KEY";

        public ErrorHandlingConfig(RequestDelegate _next)
        {
            next = _next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!context.Request.Headers.TryGetValue(ApiKey, out var apiKeyVal))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Api Key not found!");
                    return;
                }

                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var apiKey = appSettings.GetValue<string>(ApiKey);
                if (!apiKey.Equals(apiKeyVal))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized client");
                    return;
                }

                await next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception e) 
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // Customize Middleware Exceptions

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = "An internal server error occurred."
            }.ToString());
        }
    }
}
