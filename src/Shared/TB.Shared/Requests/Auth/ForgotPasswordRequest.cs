using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Auth
{
    public record ForgotPasswordRequest : Request
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PassConfirm { get; set; }
        public string? ResetUrl { get; set; }
    }
}
