using Sympli.Project.Tracking.Domains.Responses;
using System.Net;
using System.Text.Json;

namespace Sympli.Project.Tracking.Middlewares
{
    public class ApiExceptionMiddleware : AbstractExceptionHandlerMiddleware
    {
        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger) :
            base(next, logger)
        {
        }

        protected override (HttpStatusCode code, string message) GetResponse(PathString path, Exception exception)
        {
            return (HttpStatusCode.InternalServerError, JsonSerializer.Serialize(new Response<object>
            {
                Message = "Internal Server Error",
                StatusCode = HttpStatusCode.InternalServerError,
            }));
        }
    }
}