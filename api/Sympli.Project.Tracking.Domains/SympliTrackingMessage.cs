namespace Sympli.Project.Tracking.Domains
{
    public static class SympliTrackingMessage
    {
        public static class Error
        {
            public const string ErrorReadUrl = "Cannot read the content of url: {0}.";
            public const string ErrorNotFoundResultUrl = "Cannot found result in content of url: {0}.";
            public const string ErrorValueEmptyReturn = "Cannot read the content. Return value is empty.";
        }

        public static class Success
        {
            public const string SuccessSearching = "SEO result for keyword: {0} in the top 100, from {1}: {2}.";
        }
    }
}