using System.ComponentModel.DataAnnotations;

namespace TB.Domain.Models
{
    public class AuthToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? TokenName { get; set; }
        public string? TokenValue { get; set; } 
        public DateTime DateCreated { get; set; } 
        public DateTime DateUpdated { get; set; } 
    }
}
