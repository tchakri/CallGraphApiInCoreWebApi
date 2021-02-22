namespace GraphiAPI.Helpers
{
    public static class GraphConstants
    {
        public static string[] DefaultScopes = new string[] { $"https://graph.microsoft.com/.default" };
        public const string UserQueryParameter = "?$select=id,displayName,mail";
    }
}
