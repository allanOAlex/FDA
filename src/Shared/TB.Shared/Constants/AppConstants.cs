namespace TB.Shared.Constants
{
    public static class AppConstants
    {
        public static bool SessionValidityChecked { get; set; }
        public static bool TokenValidityChecked { get; set; }
        public static bool RequestValidated { get; set; }
        public static bool HasRedirected { get; set; }
        public static string? AuthToken { get; set; }
        public static string? SessionStartTime { get; set; }
        public static string? SessionTimeOut { get; set; }
        public static int? StatusCode { get; set; }
    }
}
