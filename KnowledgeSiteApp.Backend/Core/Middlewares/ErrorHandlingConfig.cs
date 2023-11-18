using KnowledgeSiteApp.Models.ErrorHandling;
using Microsoft.AspNetCore.Http;

namespace KnowledgeSiteApp.Backend.Core.ErrorHandlingMiddleware
{
    public class ErrorHandlingConfig
    {
        private readonly RequestDelegate next;

        public ErrorHandlingConfig(RequestDelegate _next)
        {
            next = _next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (InvalidOperationException e)
            {
                await HandleCustomInvalidnAsync(context, e);
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

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = "An internal server error occurred."
            }.ToString());
        }

        private static Task HandleCustomInvalidnAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = e.Message
            }.ToString());
        }

    }
}
