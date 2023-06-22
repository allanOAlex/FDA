using TB.Application.Abstractions.IRepositories;

namespace TB.Application.Abstractions.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        IAppUserRepository AppUsers { get; }
        IRoleRepository Roles { get; }
        IFinancialDataRepository FinancialData { get; }
        IEmployeeRepository Employee { get; }

        Task CompleteAsync();
    }
}
