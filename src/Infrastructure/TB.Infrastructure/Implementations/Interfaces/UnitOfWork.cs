using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Infrastructure.Implementations.Repositories;
using TB.Persistence.SQLServer;

namespace TB.Infrastructure.Implementations.Interfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAuthRepository Auth { get; private set; }
        public IAppUserRepository AppUsers { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IFinancialDataRepository FinancialData { get; private set; }

        private readonly DBContext context;
        private readonly Dappr daper;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration config;


        public UnitOfWork(DBContext Context, Dappr Daper, UserManager<AppUser> UserManager, SignInManager<AppUser> SignInManager, IConfiguration Config)
        {
            context = Context;
            daper = Daper;
            userManager = UserManager;
            signInManager = SignInManager;
            config = Config;

            Auth = new AuthRepository(signInManager, userManager, config);
            AppUsers = new AppUserRepository(context, daper, userManager);
            Roles = new RoleRepository();
            FinancialData = new FinancialDataRepository(context);
        }


        public Task CompleteAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
    }

}
