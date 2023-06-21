using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Auth
{
    public record GetRefreshTokenResponse : Response
    {
        public string? RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; } 
    }
}
