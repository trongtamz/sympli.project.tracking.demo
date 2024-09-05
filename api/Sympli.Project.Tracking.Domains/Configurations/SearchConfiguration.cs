namespace Sympli.Project.Tracking.Domains.Configurations
{
    public class SearchConfiguration
    {
        public string DefaultUserAgent { get; set; } = string.Empty;
        public string DefaultKeyword { get; set; } = string.Empty;
        public string DefaultUrl { get; set; } = string.Empty;
        public int DefaultTotalItem { get; set; }

        public string DefaultUrlSearchGoogle { get; set; } = string.Empty;
        public string DefaultUrlSearchBing { get; set; } = string.Empty;
    }
}