using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Auth
{
    public record ResetPasswordRequest : Request
    {
        public int UserId { get; set; }
        public string? Token { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
    }
}
