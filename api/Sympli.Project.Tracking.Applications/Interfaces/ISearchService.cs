using Sympli.Project.Tracking.Domains.Requests;
using Sympli.Project.Tracking.Domains.Responses;

namespace Sympli.Project.Tracking.Applications.Interfaces
{
    public interface ISearchService
    {
        Task<Response<string>> SearchAsync(GetSeoRequest request);
    }
}