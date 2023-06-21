namespace TB.Infrastructure.Configs
{
    public class JwtSettings
    {
        public JwtSettings()
        {

        }

        public int ClockSkew { get; set; }
        public string? JwtSecurityKey { get; set; }
        public string? JwtIssuer { get; set; }
        public string? JwtAudience { get; set; }
        public int JwtExpiryInMinutes { get; set; }
    }
}
