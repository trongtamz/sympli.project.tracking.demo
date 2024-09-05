namespace Sympli.Project.Tracking.Applications.Interfaces
{
    public interface IRequestValidator<TRequest>
    {
        IEnumerable<string> ValidateRequest(TRequest request);
    }
}