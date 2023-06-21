using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.User
{
    public record DeleteUserRequest : Request
    {
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public int DeletedBy { get; set; }
        public bool IsActive { get; set; }
        public bool Successful { get; set; }
        public string? Message { get; set; }
    }
}
