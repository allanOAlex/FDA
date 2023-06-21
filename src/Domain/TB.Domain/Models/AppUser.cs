using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace TB.Domain.Models
{
    public class AppUser : IdentityUser<int>
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherNames { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        [NotMapped]
        public string? Password { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime PasswordResetTokenExpiry { get; set; }

        [DefaultValue(0)]
        public bool IsDeleted { get; set; } = false;
        public DateTime DeletedOn { get; set; }
        public int DeletedBy { get; set; }


        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public List<AppUserLogin>? Logins { get; set; }
        public List<AppUserRole>? Roles { get; set; }



    }

}
