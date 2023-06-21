using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Auth
{
    public record UnlockScreenRequest : Request
    {
        public int UserId { get; set; }
        public string? Password { get; set; }
    }
}
