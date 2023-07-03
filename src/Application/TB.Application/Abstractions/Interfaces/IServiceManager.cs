using TB.Application.Abstractions.IServices;

namespace TB.Application.Abstractions.Interfaces
{
    public interface IServiceManager
    {
        
        IAuthService AuthService { get; }
        ILoggingService LoggingService { get; }
        IAppUserService AppUserService { get; }
        IRoleService RoleService { get; }
        IEmailService EmailService { get; }
        IFinancialDataService FinancialDataService { get; }
        IEmployeeService EmployeeService { get; }
        
    }
}
