using Sympli.Project.Tracking.Domains.Responses;

namespace Sympli.Project.Tracking.Infrastructures.HttpClients.Interfaces
{
    public interface ISearchEngineService
    {
        Task<Response<IEnumerable<string>>> GetContentBlock(string keywords);
    }
}