using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Auth
{
    public record ResetPasswordResponse : Response
    {
        public List<string>? Errors { get; set; }
    }
}
