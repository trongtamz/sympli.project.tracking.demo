using Microsoft.AspNetCore.Mvc;
using Sympli.Project.Tracking.Domains.Requests;
using Sympli.Project.Tracking.Domains.Responses;
using System.Net.Mime;

namespace Sympli.Project.Tracking.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ApiBaseController
    {
        public SearchController()
        {
        }

        /// <summary>
        /// Search by engine
        /// </summary>
        /// <param name="request"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchByEngineAsync([FromQuery] GetSeoRequest request)
        {
            var response = await Mediator.Send(request);
            return ProcessResponseMessage(response);
        }
    }
}