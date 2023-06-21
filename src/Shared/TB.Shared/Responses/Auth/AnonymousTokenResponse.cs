using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Autrh
{
    public record AnonymousTokenResponse : Response
    {
        public string? Token { get; set; }
    }
}
