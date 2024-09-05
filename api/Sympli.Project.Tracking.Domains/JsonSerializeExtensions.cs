namespace Sympli.Project.Tracking.Domains
{
    public static class JsonSerializeExtensions
    {
        public static async Task<string?> TryReadContentAsString(this string obj, HttpClient httpClient)
        {
            if (!Uri.IsWellFormedUriString(obj, UriKind.Absolute))
            {
                return null;
            }

            var response = await httpClient.GetAsync(obj);
            try
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}