using System.Linq.Expressions;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;

namespace TB.Infrastructure.Implementations.Repositories
{
    internal sealed class RoleRepository : IBaseRepository<AppUserRole>, IRoleRepository
    {
        public RoleRepository()
        {
                
        }

        public Task<AppUserRole> Create(AppUserRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<AppUserRole> Delete(AppUserRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<AppUserRole>> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<AppUserRole>> FindByCondition(Expression<Func<AppUserRole, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<AppUserRole?> FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<AppUserRole> Update(AppUserRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
