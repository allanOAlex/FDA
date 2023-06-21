using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TB.Domain.Models
{
    public class AppUserLogin : IdentityUserLogin<int>
    {
        public int LoginId { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        public DateTime LastLogin { get; set; }
        public DateTime LastLogout { get; set; }
        public string? IPAddress { get; set; }
        public string? ComputerName { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }



    }
}
