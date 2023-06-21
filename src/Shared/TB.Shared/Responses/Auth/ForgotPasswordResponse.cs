using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Auth
{
    public record ForgotPasswordResponse : Response
    {
        public string? Password { get; set; }
        public string? Token { get; set; }
        public string? ResetUrl { get; set; }
    }
}
