using System.Net;
using System.Net.Mime;

namespace Sympli.Project.Tracking.Middlewares
{
    public abstract class AbstractExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        protected AbstractExceptionHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        protected abstract (HttpStatusCode code, string message) GetResponse(PathString path, Exception exception);

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "{Message} - exception: {Exception} - Path {Path}", exception.Message, exception,
                    context.Request.Path.Value);
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var (status, message) = GetResponse(context.Request.Path, exception);
                context.Response.StatusCode = (int)status;
                await context.Response.WriteAsync(message);
            }
        }
    }
}