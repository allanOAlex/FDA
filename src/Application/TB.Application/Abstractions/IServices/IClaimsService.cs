using System.Security.Claims;
using TB.Domain.Models;

namespace TB.Application.Abstractions.IServices
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetUserClaimsAsync(AppUser appUser);
    }
}
