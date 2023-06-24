using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Infrastructure.Implementations.Repositories;
using TB.Persistence.MySQL.MySQL;
using TB.Persistence.SQLServer;

namespace TB.Infrastructure.Implementations.Interfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAuthRepository Auth { get; private set; }
        public IAppUserRepository AppUsers { get; private set; }
        public IEmployeeRepository Employee { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IFinancialDataRepository FinancialData { get; private set; }
        public IDividendRepository Dividend { get; private set; }
        public IEarningRepository Earning { get; private set; }
        public IStockPriceRepository StockPrice { get; private set; }
        

        private readonly DBContext context;
        private readonly MyDBContext myContext;
        private readonly Dappr daper;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration config;


        public UnitOfWork(DBContext Context, MyDBContext MyContext, Dappr Daper, UserManager<AppUser> UserManager, SignInManager<AppUser> SignInManager, IConfiguration Config)
        {
            context = Context;
            myContext = MyContext;
            daper = Daper;
            userManager = UserManager;
            signInManager = SignInManager;
            config = Config;

            Auth = new AuthRepository(signInManager, userManager, config);
            AppUsers = new AppUserRepository(context, daper, userManager);
            Employee = new EmployeeRepository(config, myContext);
            Roles = new RoleRepository();
            FinancialData = new FinancialDataRepository(context);
            Dividend = new DividendRepository(myContext);
            Earning = new EarningRepository(myContext);
            StockPrice = new StockPriceRepository(myContext);
            
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
