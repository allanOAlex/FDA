using TB.Domain.Models;

namespace TB.Application.Abstractions.IRepositories
{
    public interface IAppUserRepository : IBaseRepository<AppUser>
    {
        Task<AppUser> CreateWithUserManager(AppUser entity);

    }
}
