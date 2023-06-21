using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TB.Domain.Models
{
    public class AppUserRole : IdentityRole<int>
    {

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
