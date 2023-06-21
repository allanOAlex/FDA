using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TB.Application.Abstractions.IServices;
using TB.Domain.Models;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class ClaimsService : IClaimsService
    {
        private readonly UserManager<AppUser> userManager;
        public ClaimsService(UserManager<AppUser> UserManager)
        {
            userManager = UserManager;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(AppUser appUser)
        {
            try
            {
                List<Claim> userClaims = new()
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                    new Claim("Username", appUser.UserName),
                    new Claim("Password", appUser.PasswordHash),
                    new Claim("Firstname", appUser.FirstName!),
                    new Claim("Lastname", appUser.LastName!),
                    new Claim(ClaimTypes.Name, string.Concat(appUser.FirstName, " ", appUser.LastName)),
                    new Claim(ClaimTypes.Email, appUser.Email)
                };

                var userRoles = await userManager.GetRolesAsync(appUser);

                foreach (var userRole in userRoles)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                return userClaims;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching user claims | {ex.InnerException}");
            }
        }
    }
}
