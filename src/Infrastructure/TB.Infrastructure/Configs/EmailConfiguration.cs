namespace TB.Infrastructure.Configs
{
    public class EmailConfiguration
    {
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Password { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
    }
}
