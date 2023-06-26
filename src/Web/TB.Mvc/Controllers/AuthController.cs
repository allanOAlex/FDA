using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using TB.Application.Abstractions.Interfaces;
using TB.Domain.Models;
using TB.Mvc.AuthProviders;
using TB.Mvc.Session;
using TB.Shared.Constants;
using TB.Shared.Dtos;
using TB.Shared.Requests.Auth;
using TB.Shared.Responses.Auth;
using TB.Shared.Responses.FinancialData;

namespace TB.Mvc.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IServiceManager serviceManager;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly SessionDictionary<string> sessionDictionary;
        private IConfiguration configuration;
        private readonly HttpClient client;

        public AuthController(IServiceManager ServiceManager, AuthenticationStateProvider AuthStateProvider, SessionDictionary<string> SessionDictionary, IConfiguration Configuration, IHttpClientFactory httpClientFactory) : base(SessionDictionary)
        {
            serviceManager = ServiceManager;
            authStateProvider = AuthStateProvider;
            sessionDictionary = SessionDictionary;
            configuration = Configuration;
            client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(configuration["ApiConfig:BaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            

        }

        public IActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            try
            {
                var response = await serviceManager.AuthService.LoginWithSignInManager(loginRequest);
                if (response.Successful != true)
                {
                    StatusCode(StatusCodes.Status500InternalServerError, response);
                    return View(response);
                }

                var financialData = await serviceManager.FinancialDataService.FindAll();

                StatusCode(StatusCodes.Status200OK, response);
                AppConstants.AuthToken = response!.Token;
                AppConstants.SessionStartTime = DateTime.Now.ToString();

                HttpContext!.Session.SetInt32("SessionTimeout", 10);
                HttpContext!.Session.SetString("SessionStartTime", DateTime.Now.ToString());
                sessionDictionary["AuthToken"] = $"{response!.Token}";
                sessionDictionary["SessionStart"] = DateTime.Now.ToString("f");
                sessionDictionary["UserId"] = $"{response.Id}";
                sessionDictionary["UserName"] = $"{loginRequest!.UserName}";
                sessionDictionary["Password"] = $"{loginRequest!.Password}";
                sessionDictionary["Name"] = $"{response!.FirstName} {response!.LastName}";
                await HttpContext.Session.CommitAsync();

                
                ViewBag.Show = true;
                ViewBag.Success = response!.Successful;
                ViewBag.Message = response.Message;
                ViewBag.JwtToken = response!.Token;
                ViewBag.UserId = sessionDictionary["UserId"];
                ViewBag.Name = sessionDictionary["Name"];

                

                var authState = await ((CustomAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
                ((CustomAuthStateProvider)authStateProvider).MarkUserAsAuthenticated(response);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response!.Token);

                var isAuthenticated = authState.User.Identity!.IsAuthenticated;
                var claims = authState.User.Claims.ToDictionary(c => c.Type, c => c.Value);

                sessionDictionary["UserClaims"] = $"{authState.User.Claims}";

                return View("Dashboard", financialData);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ActionResult<LogoutResponse>> Logout(LogoutRequest request)
        {
            request.Id = int.Parse(sessionDictionary["UserId"]);
            request.UserName = sessionDictionary["UserName"];
            request.Password = sessionDictionary["Password"];

            var response = await serviceManager.AuthService.LogoutWithSignInManager(request);
            if (response.Successful != true)
            {
                StatusCode(StatusCodes.Status500InternalServerError, response);
                return View(response);
            }

            AppConstants.AuthToken = null;
            ((CustomAuthStateProvider)authStateProvider).MarkUserAsLoggedOut();
            client.DefaultRequestHeaders.Authorization = null;
            var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            var isAuthenticated = authState.User.Identity!.IsAuthenticated;
            var claims = authState.User.Claims.ToDictionary(c => c.Type, c => c.Value);
            return await HandleSessionTimeOut();


        }


        public async Task<ActionResult<UnlockScreenResponse>> UnlockScreen()
        {
            UnlockScreenRequest unlockScreenRequest = new()
            {
                UserId = int.Parse(sessionDictionary["UserId"]),
                Password = sessionDictionary["Password"]
            };

            var response = await serviceManager.AuthService.UnlockScreen(unlockScreenRequest);
            if (!response.Successful == true)
            {
                var failureResponse = new
                {
                    success = false,
                    message = "Unlock screen failed"
                };
                return Json(failureResponse);
            }

            return Json(response);

        }

        public async Task<ActionResult<GetRefreshTokenResponse>> HandleTokenExpired()
        {
            var userId = int.Parse(sessionDictionary["UserId"]);
            var refreshToken = await serviceManager.AuthService.FetchRefreshToken(userId);
            if (!string.IsNullOrEmpty(refreshToken))
            {
                GetRefreshTokenResponse response = new()
                {
                    Successful = true,
                    RefreshToken = refreshToken
                };

                return Json(response);
            }
            else
            {
                
                string userClaimsString = sessionDictionary["UserClaims"]; 
                List<Claim> userClaims = JsonConvert.DeserializeObject<List<Claim>>(sessionDictionary["UserClaims"])!;
                GetRefreshTokenRequest refreshTokenRequest = new()
                {
                    UserId = int.Parse(sessionDictionary["UserId"]),
                    RefreshTokenId = string.Join(".", Guid.NewGuid().ToString(), sessionDictionary["UserId"]),
                    ExpirationDate = DateTime.UtcNow.AddDays(7)
                };

                var response = await serviceManager.AuthService.GenerateRefreshToken(userClaims!, refreshTokenRequest);
                if (!response.Successful == true) { return Json(response); }

                return Json(response);
            }

            
        }

        public async Task<ActionResult<CreateJwtAuthTokenResponse>> CreateJwtToken()
        {
            var response = await serviceManager.AuthService.CreateJwtAuthToken(int.Parse(sessionDictionary["UserId"]));
            if (!response.Successful == true)
            {
                return Json(response);
            }

            return Json(response);

        }


        [Authorize(Policy = "AllowAnonymousAccess")]
        public IActionResult RedirectToLogin()
        {
            AppConstants.SessionValidityChecked = false;
            AppConstants.SessionStartTime = string.Empty;
            AppConstants.HasRedirected = false;
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> HandleSessionTimeOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Policy = "AllowAnonymousAccess")]
        public IActionResult HandleSessionExpired()
        {
            return View();
        }

    }
}
