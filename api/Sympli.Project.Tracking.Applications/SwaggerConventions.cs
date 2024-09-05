using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;

namespace Sympli.Project.Tracking.Applications
{
    public sealed class SwaggerConventions : IControllerModelConvention
    {
        private readonly IConfiguration _configuration;

        public SwaggerConventions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Apply(ControllerModel controller)
        {
            var version = _configuration.GetValue<string>("Version");

            controller.ApiExplorer.GroupName = $"v{version}";
            return;
        }
    }
}