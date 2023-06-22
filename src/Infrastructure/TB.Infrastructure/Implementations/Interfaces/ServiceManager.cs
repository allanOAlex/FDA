using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using TB.Domain.Models;
using TB.Infrastructure.Configs;
using TB.Infrastructure.Implementations.Services;

namespace TB.Infrastructure.Implementations.Interfaces
{
    public class ServiceManager : IServiceManager
    {
        public IAppUserService AppUserService { get; private set; }
        public IAuthService AuthService { get; private set; }
        public IRoleService RoleService { get; private set; }
        public IFinancialDataService FinancialDataService { get; private set; }
        public IEmailService EmailService { get; private set; }

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IClaimsService? claimsService;
        private readonly IJwtTokenService? jwtTokenService;
        private readonly IConfiguration config;
        private readonly EmailConfiguration emailConfig;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ServiceManager(IUnitOfWork UnitOfWork, IMapper Mapper, UserManager<AppUser> UserManager, SignInManager<AppUser> SignInManager, IClaimsService ClaimsService, IJwtTokenService JwtTokenService, IConfiguration Config, EmailConfiguration EmailConfig, IHttpContextAccessor HttpContextAccessor)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;
            userManager = UserManager;
            signInManager = SignInManager;
            claimsService = ClaimsService;
            jwtTokenService = JwtTokenService;  
            config = Config;
            emailConfig = EmailConfig;
            httpContextAccessor = HttpContextAccessor;


            AppUserService = new AppUserService(unitOfWork, mapper, userManager);
            AuthService = new AuthService(unitOfWork, mapper, signInManager, userManager, config, claimsService, jwtTokenService, httpContextAccessor);
            RoleService = new RoleService();
            FinancialDataService = new FinancialDataService(unitOfWork, mapper);
            EmailService = new EmailService(emailConfig);
        }

    }

}
