using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.User
{
    public record CreateUserResponse : Response
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        
        public IEnumerable<string>? Errors { get; set; }
    }
}
