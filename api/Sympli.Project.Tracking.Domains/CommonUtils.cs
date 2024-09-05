namespace Sympli.Project.Tracking.Domains
{
    public static class CommonUtils
    {
        public static string GetDefaulGoogleSearch(string defaultUrlSearch, string keywords, int itemPerPage = 0)
        {
            if (itemPerPage == 0)
            {
                return $"{defaultUrlSearch}/search?q={keywords}";
            }
            return $"{defaultUrlSearch}/search?q={keywords}&num={itemPerPage}";
        }

        public static string GetDefaulBingSearch(string defaultUrlSearch, string keywords, int page, int itemPerPage)
        {
            return $"{defaultUrlSearch}/search?q={keywords}&first={page * itemPerPage + 1}";
        }
    }
}