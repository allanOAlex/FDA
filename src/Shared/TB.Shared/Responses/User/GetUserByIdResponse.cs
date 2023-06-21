using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.User
{
    public record GetUserByIdResponse : Response
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        
    }
}
