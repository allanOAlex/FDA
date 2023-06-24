using TB.Application.Abstractions.IRepositories;

namespace TB.Application.Abstractions.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        IAppUserRepository AppUsers { get; }
        IEmployeeRepository Employee { get; }
        IRoleRepository Roles { get; }
        IFinancialDataRepository FinancialData { get; }
        IDividendRepository Dividend { get; }
        IEarningRepository Earning { get; }
        IStockPriceRepository StockPrice { get; }
        

        Task CompleteAsync();
    }
}
