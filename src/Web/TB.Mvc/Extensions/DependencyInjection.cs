using Microsoft.AspNetCore.Components.Authorization;
using TB.Mvc.AuthProviders;

namespace TB.Mvc.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            
            return services;

        }





    }
}
