using FluentValidation;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Components.Authorization;
using TB.Mvc.AuthProviders;
using TB.Shared.Requests.Auth;
using TB.Shared.Validations.RequestValidators;

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
