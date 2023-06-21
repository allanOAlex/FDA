using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.SQLServer;

namespace TB.Infrastructure.Implementations.Repositories
{
    internal sealed class AppUserRepository : IBaseRepository<AppUser>, IAppUserRepository
    {
        private readonly DBContext context;
        private readonly Dappr daper;
        private readonly UserManager<AppUser> userManager;

        public AppUserRepository(DBContext Context, Dappr Daper, UserManager<AppUser> UserManager)
        {
            context = Context;
            daper = Daper;
            userManager = UserManager;
        }

        public async Task<AppUser> Create(AppUser entity)
        {
            await context.AppUsers!.AddAsync(entity);
            return entity;
        }

        public async Task<AppUser> CreateWithUserManager(AppUser entity)
        {
            try
            {
                await userManager.CreateAsync(entity, entity.Password);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(AppUserRepository)}(CreateWithUserManager)Error Creating User {ex.Message}");
            }
        }

        public async Task<AppUser> Delete(AppUser entity)
        {
            var userToDelete = await context.AppUsers!.FindAsync(entity.Id);
            if (userToDelete != null)
            {
                context.AppUsers.Remove(userToDelete);
                await context.SaveChangesAsync();
                return userToDelete;
            }
            
            return entity!;
        }

        public async Task<IQueryable<AppUser>> FindAll()
        {
            return await Task.FromResult(context.AppUsers!.OrderByDescending(e => e.Id).AsNoTracking());
        }

        public async Task<IQueryable<AppUser>> FindByCondition(Expression<Func<AppUser, bool>> expression)
        {
            return await Task.FromResult(context.AppUsers!.Where(expression).AsNoTracking());
        }

        public async Task<AppUser?> FindById(int Id)
        {
            return await context.AppUsers!.FindAsync(Id);
        }

        public async Task<AppUser> Update(AppUser entity)
        {
            try
            {
                if (context.ChangeTracker.Entries<AppUser>().Any(e => e.Entity!.Id == entity!.Id))
                {
                    context.DetachAllEntities();
                    var userToUpdate = await context.AppUsers!.FindAsync(entity.Id);
                    context.Entry(userToUpdate).State = EntityState.Detached;
                    userToUpdate = entity;
                    AppUser user = userToUpdate;
                    context.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    context.AppUsers!.Update(entity);
                }

                return entity;

            }
            catch (Exception )
            {
                throw;
            }
        }
    }
}
