using TB.Application.Abstractions.IRepositories;

namespace TB.Application.Abstractions.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        ILoggingRepository Logging { get; }
        ICacheRepository Cache { get; }
        IAppUserRepository AppUser { get; }
        IEmployeeRepository Employee { get; }
        IRoleRepository Role { get; }
        IFinancialDataRepository FinancialData { get; }
        IDividendRepository Dividend { get; }
        IEarningRepository Earning { get; }
        IStockPriceRepository StockPrice { get; }
        

        Task CompleteAsync();
    }
}
