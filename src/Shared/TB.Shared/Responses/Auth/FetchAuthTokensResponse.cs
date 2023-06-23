using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Auth
{
    public record FetchAuthTokensResponse : Response
    {
        public int UserId { get; set; }
        public string? TokenName { get; set; }
        public string? TokenValue { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


    }
}
