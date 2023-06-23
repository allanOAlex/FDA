using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Auth
{
    public record TokenExpiredRequest : Request
    {
        public int UserId { get; set; }
    }
}
