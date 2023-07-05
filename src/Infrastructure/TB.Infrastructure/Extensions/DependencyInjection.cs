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
using Microsoft.Extensions.Configuration;
using Serilog.Formatting.Json;
using System.Net;

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

        public class IPAddressEnricher1 : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                var ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (ipAddress != null)
                {
                    var ipAddressProperty = propertyFactory.CreateProperty("IPAddress", ipAddress);
                    logEvent.AddPropertyIfAbsent(ipAddressProperty);
                }
            }
        }

        class IPAddressEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                var httpContext = httpContextAccessor.HttpContext;
                var remoteIpAddress = httpContext?.Connection?.RemoteIpAddress;

                // Check if the IP address is available and not in IPv6 loopback format
                if (remoteIpAddress != null && !IPAddress.IsLoopback(remoteIpAddress))
                {
                    var ipAddress = remoteIpAddress.ToString();
                    var ipAddressProperty = propertyFactory.CreateProperty("IPAddress", ipAddress);
                    logEvent.AddPropertyIfAbsent(ipAddressProperty);
                }
            }
        }



        private static string GetLogFilePath(IConfiguration configuration)
        {
            string logFilePath = configuration["Logging:Serilog:LogFile"]!;
            string logFileName = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss}.txt";
            var filePath = Path.Combine(logFilePath, logFileName);
            return filePath;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            
            services.AddScoped<ILoggingRepository, LoggingRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IFinancialDataRepository, FinancialDataRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFinancialDataService, FinancialDataService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information() 
            .MinimumLevel.Override("Serilog", LogEventLevel.Warning) 
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
            .Enrich.FromLogContext()
            .Enrich.With(new CorrelationIdEnricher())
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.With<IPAddressEnricher>()
            .Filter.ByExcluding(logEvent =>
            {
                if (logEvent.Properties.TryGetValue("SourceContext", out var value) &&
                    value is ScalarValue scalarValue &&
                    scalarValue.Value is string sourceContext)
                {
                    return sourceContext.StartsWith("Microsoft.");
                }

                return false;
            })
            .WriteTo.Async(s => s.Console(new CompactJsonFormatter()))
            .WriteTo.Async(s => s.File(new JsonFormatter(), configuration["Logging:Serilog:LogFile"]!, rollingInterval: RollingInterval.Day))
            .CreateLogger();

            services.AddSingleton<ILoggerFactory>(provider =>
            {
                return new SerilogLoggerFactory(Log.Logger, true);
            });

            services.AddMemoryCache();


            return services;


        }
    }

}
