using KnowledgeSiteApp.Models.ErrorHandling;

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
            var errorDetails = new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = e.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorDetails.StatusCode;

            return context.Response.WriteAsync(errorDetails.ToString());
        }

        private static Task HandleCustomInvalidnAsync(HttpContext context, Exception e)
        {
            var errorDetails = new ErrorDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = e.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorDetails.StatusCode;

            return context.Response.WriteAsync(errorDetails.ToString());
        }

    }
}
