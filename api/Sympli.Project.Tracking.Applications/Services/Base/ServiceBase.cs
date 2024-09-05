using Sympli.Project.Tracking.Domains.Responses;
using System.Net;

namespace Sympli.Project.Tracking.Applications.Services.Base
{
    public abstract class ServiceBase : IServiceBase
    {
        protected ServiceBase()
        {
        }

        public Response<T> BuildResponse<T>(T data, HttpStatusCode statusCode, string message)
        {
            return new Response<T>
            {
                Data = data,
                Message = message,
                StatusCode = statusCode,
            };
        }

        public Response<T> BuildSuccessResponse<T>(T data, string message)
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Message = message
            };
        }

        public Response<T> BuildSuccessResponse<T>(T data)
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Message = "Success"
            };
        }
    }
}