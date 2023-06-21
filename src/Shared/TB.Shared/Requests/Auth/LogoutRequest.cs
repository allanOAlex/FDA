using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Auth
{
    public record LogoutRequest : Request
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
