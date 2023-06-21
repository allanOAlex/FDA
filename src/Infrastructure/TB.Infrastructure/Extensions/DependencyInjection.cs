using Microsoft.Extensions.DependencyInjection;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IRepositories;
using TB.Application.Abstractions.IServices;
using TB.Infrastructure.Implementations.Interfaces;
using TB.Infrastructure.Implementations.Repositories;
using TB.Infrastructure.Implementations.Services;

namespace TB.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IFinancialDataRepository, FinancialDataRepository>();

            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFinancialDataService, FinancialDataService>();

            return services;


        }
    }

}
