using System.ComponentModel.DataAnnotations;

namespace TB.Domain.Models
{
    public class AppUserToken
    {
        [Key]
        public int Id { get; set; } 
        public int AppUserId { get; set; } 
        public string? LoginProvider { get; set; } 
        public string? TokenName { get; set; } 

        public AppUser? AppUser { get; set; }
    }
}
