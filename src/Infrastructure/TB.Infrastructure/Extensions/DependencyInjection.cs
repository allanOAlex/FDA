using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IRepositories;
using TB.Application.Abstractions.IServices;
using TB.Infrastructure.Implementations.Interfaces;
using TB.Infrastructure.Implementations.Repositories;
using TB.Infrastructure.Implementations.Services;
using Serilog.Formatting.Compact;
using Serilog.Extensions.Logging;
using Serilog.Core;
using Microsoft.AspNetCore.Http;

namespace TB.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        private static IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        public class CorrelationIdEnricher : ILogEventEnricher
        {
            private const string CorrelationIdPropertyName = "CorrelationId";

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                var correlationId = GetCorrelationId(); // Implement your logic to retrieve or generate a correlation ID
                var correlationIdProperty = new LogEventProperty(CorrelationIdPropertyName, new ScalarValue(correlationId));

                logEvent.AddOrUpdateProperty(correlationIdProperty);
            }

            private string GetCorrelationId()
            {
                // Retrieve the correlation ID from the HTTP context if available
                var httpContext = httpContextAccessor.HttpContext;
                var correlationId = httpContext?.Request.Headers["CorrelationId"].FirstOrDefault();

                // If the correlation ID is not available in the HTTP context, generate a new GUID
                if (string.IsNullOrEmpty(correlationId))
                {
                    correlationId = Guid.NewGuid().ToString();
                }

                return correlationId;
            }
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IFinancialDataRepository, FinancialDataRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFinancialDataService, FinancialDataService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information() // Set the minimum logging level
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Override the minimum level for Microsoft logs
            .Enrich.FromLogContext()
            .Enrich.With(new CorrelationIdEnricher())
            .WriteTo.Console(new CompactJsonFormatter()) // Add the console sink
            .WriteTo.File(new RenderedCompactJsonFormatter(), "D:\\AppData\\Logs\\TBSDE\\log.txt", rollingInterval: RollingInterval.Day) // Add a file sink
            .CreateLogger();

            services.AddSingleton<ILoggerFactory>(provider =>
            {
                return new SerilogLoggerFactory(Log.Logger, true);
            });




            return services;


        }
    }

}
