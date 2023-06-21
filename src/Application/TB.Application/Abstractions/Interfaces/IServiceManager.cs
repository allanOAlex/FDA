using TB.Application.Abstractions.IServices;

namespace TB.Application.Abstractions.Interfaces
{
    public interface IServiceManager
    {
        IAppUserService AppUserService { get; }
        IAuthService AuthService { get; }
        IRoleService RoleService { get; }
        IEmailService EmailService { get; }
        IFinancialDataService FinancialDataService { get; }
        IGoogleService GoogleService { get; }
        
    }
}
