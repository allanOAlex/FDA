using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Auth
{
    public record GetRefreshTokenRequest : Request
    {
        public int UserId { get; set; }
        public string? RefreshTokenId { get; set; }
        public DateTime ExpirationDate { get; set; }


    }
}
