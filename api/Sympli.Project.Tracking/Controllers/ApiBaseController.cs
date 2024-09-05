using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sympli.Project.Tracking.Domains.Responses;
using System.Net;

namespace Sympli.Project.Tracking.Controllers
{
    public class ApiBaseController : Controller
    {
        private ISender? _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IActionResult ProcessResponseMessage<TU>(Response<TU> response)
        {
            HttpStatusCode statusCode = response.StatusCode;

            ObjectResult result = statusCode switch
            {
                HttpStatusCode.OK => Ok(response),
                HttpStatusCode.BadRequest => BadRequest(response),
                HttpStatusCode.UnprocessableEntity => UnprocessableEntity(response),
                HttpStatusCode.Unauthorized => Unauthorized(response),
                HttpStatusCode.Forbidden => StatusCode(403, response),
                HttpStatusCode.NotFound => NotFound(response),
                _ => StatusCode(500, response),
            };

            return result;
        }
    }
}