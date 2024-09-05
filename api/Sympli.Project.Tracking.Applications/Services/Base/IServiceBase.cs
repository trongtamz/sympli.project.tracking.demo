using Sympli.Project.Tracking.Domains.Responses;
using System.Net;

namespace Sympli.Project.Tracking.Applications.Services.Base
{
    public interface IServiceBase
    {
        Response<T> BuildResponse<T>(T data, HttpStatusCode statusCode, string message);

        Response<T> BuildSuccessResponse<T>(T data, string message);

        Response<T> BuildSuccessResponse<T>(T data);
    }
}