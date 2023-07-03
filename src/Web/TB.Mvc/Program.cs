using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TB.Domain.Models;
using TB.Infrastructure.Configs;
using TB.Infrastructure.Extensions;
using TB.Mvc.Controllers;
using TB.Mvc.Extensions;
using TB.Mvc.Session;
using TB.Persistence.MySQL.MySQL;
using TB.Persistence.SQLServer;
using TB.Shared.Validations.Helpers;


var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Auth").GetSection("Jwt").Get<JwtSettings>();
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
var SSConnString = builder.Configuration.GetConnectionString("TBSS");
var MSConnString = builder.Configuration.GetConnectionString("TBMS");

IdentityModelEventSource.ShowPII = true;

builder.Services.AddSingleton(emailConfig);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(SSConnString!));
builder.Services.AddDbContext<MyDBContext>(options =>
{
    options.UseMySql(MSConnString, ServerVersion.AutoDetect(MSConnString));
});
builder.Services.AddSingleton<Dappr>();

builder.Services.AddWebEncoders();
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

//builder.Services.AddValidation();

builder.Services.AddControllersWithViews().AddApplicationPart(typeof(ErrorController).Assembly); ;

builder.Services.AddValidatorsFromAssemblyContaining<SharedValidatorAssembly>();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    return new SessionDictionary<string>(httpContextAccessor, "AppSession");
});


builder.Services.AddIdentity<AppUser, AppUserRole>().AddEntityFrameworkStores<DBContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = "Username";
    //options.Stores.ProtectPersonalData = true;

    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Cookie settings
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;

});

builder.Services.AddAuthentication((options) =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(options =>
{

    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = jwtSettings!.JwtIssuer,
        ValidAudience = jwtSettings.JwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtSecurityKey!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        LifetimeValidator = (notBefore, expires, token, parameters) =>
        {
            return expires > DateTime.Now;
        }

    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context => {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.StatusCode = 1002;
                context.Response.Headers.Add("TokenExpired", "true");
            }
            return Task.CompletedTask;
        }
    };


});

builder.Services.AddAuthorization(options =>
{
    //options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("Admin", policy =>
        policy.RequireRole("SuperAdmin, Admin"));

    options.AddPolicy("AllowAnonymousAccess", policy =>
    {
        policy.RequireAssertion(context => !context.User.Identity!.IsAuthenticated).Build();
    });


});


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "AppSessionCookie";
    options.IdleTimeout = TimeSpan.FromSeconds(172800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddClientServices();
builder.Services.AddInfrastructure(builder.Configuration);

//builder.Host.UseSerilog((ctx, lc) => lc
//        .WriteTo.Console());



var app = builder.Build();



app.UseSession();

app.ConfigureGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    var options = new DeveloperExceptionPageOptions
    {
        SourceCodeLineCount = 2
    };

    app.UseDeveloperExceptionPage(options);
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Default",
        pattern: "{controller=Home}/{action=Index}/{viewName?}");

    endpoints.MapControllerRoute(
        name: "UnlockScreen",
        pattern: "{controller=Auth}/{action=UnlockScreen}",
        defaults: new { controller = "Auth", action = "UnlockScreen" });

    endpoints.MapControllerRoute(
        name: "Error",
        pattern: "Error/HandleStatusCode/{statusCode?}",
        defaults: new { controller = "Error", action = "HandleStatusCode" });

    endpoints.MapControllerRoute(
        name: "ResetPasswordView",
        pattern: "User/ResetPasswordView/{userId}/{token}",
        defaults: new { controller = "User", action = "ResetPasswordView" }
    );

    
});

app.Run();
