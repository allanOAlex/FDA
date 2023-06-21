using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using TB.Shared.Constants;
using TB.Shared.Responses.Auth;

namespace TB.Mvc.AuthProviders
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private readonly HttpClient httpClient;
        private readonly IConfiguration config;

        public CustomAuthStateProvider(HttpClient HttpClient, IConfiguration Config)
        {
            httpClient = HttpClient;
            config = Config;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(AppConstants.AuthToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppConstants.AuthToken);
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(AppConstants.AuthToken!), "jwt"))));

            }
            catch (Exception)
            {
                throw;
            }

        }

        public void MarkUserAsAuthenticated(string username)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsAuthenticated(LoginResponse user)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ),
                new Claim("Username", user.UserName! ),
                new Claim("Firstname", user.FirstName!),
                new Claim("Lastname", user.LastName!),
                new Claim(ClaimTypes.Name, string.Concat(user.FirstName, " ",  user.LastName))
            }, "apiauth"));

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void DoMarkUserAsAuthenticated(LoginResponse user)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Name, string.Concat(user.FirstName, " ",  user.LastName)),
            }, "apiauth"));

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var claims = new List<Claim>();
                var userClaims = new Dictionary<string, List<Claim>>();

                if (!string.IsNullOrEmpty(jwt) && jwt != "anonymous")
                {
                    var header = jwt.Split('.')[0];
                    var payload = jwt.Split('.')[1];
                    var signature = jwt.Split('.')[2];
                    var jsonBytes = ParseBase64WithoutPadding(payload);
                    var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                    var keyValuePairs1 = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

                    keyValuePairs!.TryGetValue(ClaimTypes.Role, out object? roles);
                    keyValuePairs1!.TryGetValue("claims", out object? jwtClaims);

                    if (roles != null)
                    {
                        if (roles.ToString()!.Trim().StartsWith("["))
                        {
                            var parsedRoles = System.Text.Json.JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                            foreach (var parsedRole in parsedRoles!)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                            }
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                        }

                        keyValuePairs.Remove(ClaimTypes.Role);
                    }

                    if (jwtClaims != null)
                    {
                        if (jwtClaims.ToString()!.Trim().StartsWith("["))
                        {
                            var jsonArray = JArray.Parse(jwtClaims.ToString()!);
                            foreach (var data in jsonArray)
                            {
                                var type = (string)data["Type"]!;
                                var value = (string)data["Value"]!;

                                if (type == ClaimTypes.Role)
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, value));
                                }

                            }
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, jwtClaims.ToString()!));
                        }

                        keyValuePairs.Remove(ClaimTypes.Role);
                    }

                    claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));

                    return claims;
                }
                else
                {
                    //claims.Add(new Claim(ClaimTypes.Name, "Anonymous"));
                    //claims.Add(new Claim(ClaimTypes.Role, "User"));
                    return claims;
                }


            }
            catch (Exception)
            {

                throw;
            }

        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }


    }

}
