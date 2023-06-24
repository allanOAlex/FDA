using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using TB.Domain.Models;
using TB.Infrastructure.Extensions;
using TB.Shared.Exceptions.ModelExceptions;
using TB.Shared.Requests.Auth;
using TB.Shared.Responses.Auth;
using TB.Shared.Responses.Autrh;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IClaimsService claimsService;
        private readonly IJwtTokenService jwtTokenService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthService(IUnitOfWork UnitOfWork, IMapper Mapper, SignInManager<AppUser> SignInManager, UserManager<AppUser> UserManager, IConfiguration Configuration, IClaimsService ClaimsService, IJwtTokenService JwtTokenService, IHttpContextAccessor HttpContextAccessor)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;
            signInManager = SignInManager;
            userManager = UserManager;
            configuration = Configuration;
            claimsService = ClaimsService;
            jwtTokenService = JwtTokenService;
            httpContextAccessor = HttpContextAccessor;
        }

        public async Task<CreateJwtAuthTokenResponse> CreateJwtAuthToken(int UserId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(UserId.ToString());
                switch (user)
                {
                    case null:
                        return new CreateJwtAuthTokenResponse { Successful = false, Message = "Username does not exist.", JwtAuthToken = string.Empty  };

                    default:
                        var userClaims = await claimsService.GetUserClaimsAsync(user);
                        var userToken = jwtTokenService.GetJwtToken(userClaims);
                        if (!IsTokenValid(userToken))
                        {
                            throw new SecurityTokenValidationException($"Error|Token is Invalid");
                        }

                        return new CreateJwtAuthTokenResponse { Successful = true, Message = "JwtAuthToken generated successfully!", JwtAuthToken = new JwtSecurityTokenHandler().WriteToken(userToken) };
                }

            }
            catch (Exception)
            {

                throw;
            }

            throw new NotImplementedException();
        }

        public async Task<string> FetchRefreshToken(int UserId)
        {
            try
            {
                var refreshToken = await unitOfWork.Auth.FetchRefreshTokenAsync(UserId);
                var jwtToken = new JwtSecurityTokenHandler().ReadToken(refreshToken) as JwtSecurityToken;
                bool isExpired = DateTime.UtcNow > jwtToken!.ValidTo;
                if (isExpired)
                {
                    throw new SecurityTokenExpiredException("RefreshToken has expired.");
                }
                return refreshToken;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ForgotPasswordResponse> ForgotUserPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(forgotPasswordRequest.Email);
                if (user == null)
                {
                    return new ForgotPasswordResponse { Successful = false, Message = "NotFound|Sorry, we could not find a user with the specified email" };
                }

                //AuthExtensions.GeneratePasswordResetToken(forgotPasswordRequest.Email!, out string passwordResetToken);
                var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

                var request = new MapperConfiguration(cfg => cfg.CreateMap<ForgotPasswordRequest, AppUser>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember) => srcMember != null && !srcMember.Equals(destMember))));
                var response = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, ForgotPasswordResponse>());

                IMapper requestMap = request.CreateMapper();
                IMapper responseMap = response.CreateMapper();

                var destination = requestMap.Map(forgotPasswordRequest, user);
                destination.PasswordResetToken = passwordResetToken;
                destination.PasswordResetTokenExpiry = DateTime.Now;

                try
                {
                    var appUser = unitOfWork.AppUsers.Update(destination);
                    await unitOfWork.CompleteAsync();
                    var Successful = true;

                    passwordResetToken = passwordResetToken.Replace("+", "__PLUS__");
                    var encodedToken = WebUtility.UrlEncode(passwordResetToken);

                    return Successful == true ? new ForgotPasswordResponse
                    {
                        Successful = true,
                        Id = user.Id,
                        Token = encodedToken,
                        ResetUrl = $"{forgotPasswordRequest.ResetUrl}{user.Id}/{encodedToken}"
                    } : new ForgotPasswordResponse { Successful = false, Message = "Error while trying to reset your password.", Id = user.Id, Token = passwordResetToken };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AnonymousTokenResponse> GenerateAnonymousToken()
        {
            try
            {
                var token = await Task.FromResult(jwtTokenService.GetAnonJwtToken());
                return token != null ? new AnonymousTokenResponse{Token = new JwtSecurityTokenHandler().WriteToken(token), Successful = true } : new AnonymousTokenResponse { Token = null, Successful = false };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GetRefreshTokenResponse> GenerateRefreshToken(List<Claim> userClaims, GetRefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                var user = await userManager.FindByIdAsync(refreshTokenRequest.UserId.ToString());
                switch (user)
                {
                    case null:
                        return new GetRefreshTokenResponse { Successful = false, Message = "User does not exist." };

                    case not null:
                        var refreshToken = jwtTokenService.GenerateRefreshToken(userClaims, refreshTokenRequest);
                        if (!IsTokenValid(refreshToken))
                        {
                            throw new SecurityTokenValidationException($"Error|RefreshToken is Invalid");
                        }

                        return new GetRefreshTokenResponse { Successful = true, Message = "Refresh Token Successful!", RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken), Id = user.Id };
                }

                
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool IsTokenValid(SecurityToken token)
        {
            try
            {
                if (token == null)
                {
                    throw new ArgumentException("Token is null");
                }

                AuthExtensions.SecurityKey(out string key);
                var clockSkew = Convert.ToDouble(configuration["Auth:Jwt:ClockSkew"]);
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(clockSkew),
                    ValidIssuer = configuration["Auth:Jwt:JwtIssuer"],
                    ValidAudience = configuration["Auth:Jwt:JwtAudience"],
                    IssuerSigningKey = token.SigningKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                };

                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(jwtSecurityTokenHandler.WriteToken(token), tokenValidationParameters, out var securityToken);

                    return true;
                }
                catch (SecurityTokenValidationException ex)
                {
                    throw new SecurityTokenValidationException($"Error|Token validation failed|{ex.Message}");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LoginResponse> LoginWithSignInManager(LoginRequest loginRequest)
        {
            try
            {
                var user = await userManager.FindByNameAsync(loginRequest.UserName);
                switch (user)
                {
                    case null:
                        return new LoginResponse { Successful = false, Message = "Username does not exist." };

                    case not null:
                        var isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequest.Password);

                        switch (isPasswordValid)
                        {
                            case false:
                                return new LoginResponse { Successful = false, Message = "Invalid username / password combination." };
                        }

                        break;

                }

                var userClaims = await claimsService.GetUserClaimsAsync(user);

                if (loginRequest.RememberMe == true)
                {
                    DateTime expirationTime;
                    expirationTime = DateTime.UtcNow.AddDays(7);
                    var refreshToken = jwtTokenService.GetJwtTokenWithNewExpiry(userClaims, expirationTime);

                    if (!IsTokenValid(refreshToken))
                    {
                        throw new SecurityTokenValidationException($"Error|Token is Invalid");
                    }

                    var response = httpContextAccessor.HttpContext.Response;
                    response.Cookies.Append("access_token", new JwtSecurityTokenHandler().WriteToken(refreshToken), new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = expirationTime,
                        IsEssential = true
                    });

                    var signInResult = await signInManager.PasswordSignInAsync(user.UserName, loginRequest.Password, loginRequest.RememberMe, false);

                    return signInResult.Succeeded ? new LoginResponse { Id = user.Id, Successful = true, Message = "Login Successful!", Token = new JwtSecurityTokenHandler().WriteToken(refreshToken), UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, IsAuthenticated = true } : new LoginResponse { Successful = false, Message = "Invalid username/ password combination", IsAuthenticated = false };
                }
                else
                {
                    var result = await signInManager.PasswordSignInAsync(user.UserName, loginRequest.Password, loginRequest.RememberMe, false);

                    if (!result.Succeeded)
                    {
                        return new LoginResponse { Successful = false, Message = "Invalid username/ password combination", IsAuthenticated = false };
                    }

                    var userToken = jwtTokenService.GetJwtToken(userClaims);
                    if (!IsTokenValid(userToken))
                    {
                        throw new SecurityTokenValidationException($"Error|Token is Invalid");
                    }

                    try
                    {
                        GetRefreshTokenRequest getRefreshTokenRequest = new()
                        {
                            UserId = user.Id,
                            ExpirationDate = DateTime.Now.AddMinutes(14400)
                        };

                        var refreshToken = await GenerateRefreshToken(userClaims, getRefreshTokenRequest);

                        if (!refreshToken.Successful == true)
                        {
                            throw new RefreshTokenCreationExcepion();
                        }

                        AuthToken authToken = new()
                        {
                            UserId = user.Id,
                            TokenName = "RT",
                            TokenValue = refreshToken.RefreshToken,
                            DateCreated = DateTime.UtcNow,
                            DateUpdated = DateTime.UtcNow,
                        };

                        IEnumerable<AuthToken> authTokens = await unitOfWork.Auth.FindAuthTokens();
                        var existingToken = authTokens.AsQueryable().Where(row =>
                        row.UserId.Equals(user.Id) &&
                        row.TokenName!.Equals(authToken.TokenName) //&&
                        //row.TokenValue!.Equals(authToken.TokenValue)

                        );

                        if (existingToken.Any())
                        {
                            if (!string.IsNullOrEmpty(existingToken.FirstOrDefault()!.TokenValue))
                            {
                                return new LoginResponse { Successful = true, Message = "Login Successful!", Token = new JwtSecurityTokenHandler().WriteToken(userToken), Id = user.Id, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, IsAuthenticated = true };
                            }
                            
                        }
                            
                        var createAuthTokenResult = await unitOfWork.Auth.CreateAuthTokenAsync(authToken);

                        return createAuthTokenResult != 0 ? new LoginResponse { Successful = true, Message = "Login Successful!", Token = new JwtSecurityTokenHandler().WriteToken(userToken), Id = user.Id, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, IsAuthenticated = true } : new LoginResponse { Successful = false, Message = "Error Creating AuthToken for User", Id = user.Id, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, IsAuthenticated = false };
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LogoutResponse> LogoutWithSignInManager(LogoutRequest logoutRequest)
        {
            try
            {
                var userId = await userManager.FindByIdAsync(logoutRequest.Id.ToString());
                var userName = await userManager.FindByNameAsync(logoutRequest.UserName);
                if (userId == null && userName == null)
                {
                    return new LogoutResponse { Id = logoutRequest.Id, Successful = false, Message = "Logout unsucessful - Please confirm user details." };
                }

                if (userId != null && userName == null)
                {
                    return new LogoutResponse { Id = logoutRequest.Id, Successful = false, Message = $"Logout unsucessful - User with Username '{logoutRequest.UserName}' does not seem to exist!" };
                }

                if (userId == null && userName != null)
                {
                    return new LogoutResponse { Id = logoutRequest.Id, Successful = false, Message = $"Logout unsucessful - User with ID '{logoutRequest.Id}' does not seem to exist!" };
                    
                }

                await unitOfWork.Auth.LogoutWithSignInManager();
                bool Successful = true;

                return Successful == true ? new LogoutResponse { Successful = true, Message = "Success." } : new LogoutResponse { Successful = false, Message = "Logout unsucessful. Please contact system administrator." };

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UnlockScreenResponse> UnlockScreen(UnlockScreenRequest unlockScreenRequest)
        {
            try
            {
                var user = await userManager.FindByIdAsync(unlockScreenRequest.UserId.ToString());
                if (user == null)
                {
                    return new UnlockScreenResponse { Successful = false, Message = "NotFound|Sorry, somehow we could not find this user" };
                }

                var isPasswordValid = await userManager.CheckPasswordAsync(user, unlockScreenRequest.Password);
                if (!isPasswordValid == true)
                {
                    return new UnlockScreenResponse { Successful = false, Message = "Password is not valid." };
                }

                return new UnlockScreenResponse { Successful = true, Message = "Welcome back!." };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResetPasswordResponse> UserPasswordReset(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                var user = await userManager.FindByIdAsync(resetPasswordRequest.UserId.ToString());
                if (user == null)
                {
                    return new ResetPasswordResponse { Successful = false, Message = "NotFound|Sorry, somehow we could not find this user" };
                }

                if (DateTime.Now > user.PasswordResetTokenExpiry)
                {
                    return new ResetPasswordResponse { Successful = false, Message = "Password reset link has expired. Please generate a new one on the forgot password page." };
                }

                var decodedToken = WebUtility.UrlDecode(resetPasswordRequest.Token!);
                decodedToken = decodedToken.Replace("__PLUS__", "+");
                if (decodedToken != user.PasswordResetToken)
                {
                    return new ResetPasswordResponse { Successful = false, Message = $"Invalid password reset key" };
                }

                var result = await unitOfWork.Auth.InvalidatePasswordResetToken(user);
                if (result != 1)
                {
                    return new ResetPasswordResponse { Successful = false, Message = $"PRT-IVE|Error invalidating password reset token" };
                }

                var passResetResult = await userManager.ResetPasswordAsync(user, user.PasswordResetToken, resetPasswordRequest.Password!);

                return passResetResult.Succeeded ? new ResetPasswordResponse { Successful = true, Message = "Password reset successful!" } : new ResetPasswordResponse { Successful = false, Message = "Error while trying to reset your password." };


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PassResetTokenValidationResponse> ValidateJwtPasswordResetToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var payload = tokenHandler.ReadJwtToken(token);
                var email = payload.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                var expirationDateUnix = long.Parse(payload.Claims.FirstOrDefault(c => c.Type == "exp")?.Value!);
                var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expirationDateUnix);
                var currentTime = DateTimeOffset.UtcNow;
                if (expirationDateTime <= currentTime)
                {
                    return new PassResetTokenValidationResponse { Successful = false, Message = $"Password reset key is expired. Please regenerate at Forgot Password" };
                }

                var user = await userManager.FindByEmailAsync(email);
                if (user.PasswordResetToken != token)
                {
                    return new PassResetTokenValidationResponse { Successful = false, Message = $"Invalid password reset key" };
                }

                return new PassResetTokenValidationResponse { Successful = true, Message = $"Valid" };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<string> ValidatePasswordResetToken(string token)
        {
            try
            {
                byte[] tokenBytes = WebEncoders.Base64UrlDecode(token);
                string decodedToken = Encoding.UTF8.GetString(tokenBytes);

                return Task.FromResult(decodedToken);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
