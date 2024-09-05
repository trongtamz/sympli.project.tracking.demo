namespace Sympli.Project.Tracking.Domains
{
    public static class Constants
    {
        public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36";
        public const string CachedKeySearchingSEO = "SEO_Searching_{0}_{1}_{2}";

        public const string PatternResultGoogleSearch = @"<div[^>]*id=""rso""((?'nested'<div)|</div(?'-nested')|[\w\W]*?)*</div>";
        public const string PatternHrefTagGoogleSearch = @"href\s*=\s*[""'](?<url>https?:[^\s""']+)[""']";
        public const string PatternResultBingSearch = @"<ol[^>]*id=""b_results""((?'nested'<div)|</div(?'-nested')|[\w\W]*?)*</div>";
        public const string PatternCiteTagBingSearch = @"<cite>(?<url>https?:[^\s""']+)<\/cite>";
    }
}