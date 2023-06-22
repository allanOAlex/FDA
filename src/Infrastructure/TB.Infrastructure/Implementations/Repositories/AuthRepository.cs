using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Shared.Responses.Auth;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TB.Infrastructure.Implementations.Repositories
{
    internal sealed class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration configuration;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        public AuthRepository(SignInManager<AppUser> SignInManager, UserManager<AppUser> UserManager, IConfiguration Configuration)
        {
            signInManager = SignInManager;
            userManager = UserManager;
            configuration = Configuration;
        }

        public async Task<int> ManageAuthTokenAsync(AuthToken entity, int commandId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("TBSS")))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("CommandId", commandId, DbType.Int32);
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("TokenName", entity.TokenName, DbType.String);
                    parameters.Add("TokenName", entity.TokenValue, DbType.String);
                    parameters.Add("DateCreated", entity.DateCreated, DbType.DateTime);
                    parameters.Add("DateUpdated", entity.DateUpdated, DbType.DateTime);

                    var result = await connection.ExecuteAsync("ManageAuthToken", parameters, commandType: CommandType.StoredProcedure);
                    return result;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> CreateAuthTokenAsync(AuthToken entity)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("TBSS")))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("UserId", entity.UserId, DbType.Int32);
                    parameters.Add("TokenName", entity.TokenName, DbType.String);
                    parameters.Add("TokenValue", entity.TokenValue, DbType.String);
                    parameters.Add("DateCreated", entity.DateCreated, DbType.DateTime);
                    parameters.Add("DateUpdated", entity.DateUpdated, DbType.DateTime);

                    var result = await connection.ExecuteAsync("InsertAuthToken", parameters, commandType: CommandType.StoredProcedure);
                    return result;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<FetchAuthTokensResponse>> FetchAuthTokens()
        {
            try
            {
                var query = $"SELECT * FROM [AuthTokens]";
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("TBSS")))
                {
                    connection.Open();
                    var results = await connection.QueryAsync<AuthToken>(query);
                    List<FetchAuthTokensResponse> fetchAuthTokensResponses = new();
                    if (results.Any())
                    {
                        foreach (var result in results)
                        {
                            FetchAuthTokensResponse response = new()
                            {
                                Id = result.Id,
                                UserId = result.UserId,
                                TokenName = result.TokenName,
                                TokenValue = result.TokenValue,
                                DateCreated = result.DateCreated,
                                DateUpdated = result.DateUpdated,
                            };

                            fetchAuthTokensResponses.Add(response);
                        }
                    }
                    
                    return fetchAuthTokensResponses;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<AuthToken>> FindAuthTokens()
        {
            try
            {
                var query = $"SELECT * FROM [AuthTokens]";
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("TBSS")))
                {
                    connection.Open();
                    var results = await connection.QueryAsync<AuthToken>(query);
                    return results;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> FetchRefreshTokenAsync(int userId)
        {
            try
            {
                var query = $"SELECT [TokenValue] FROM [AuthTokens] WHERE UserId = {userId}";
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("TBSS")))
                {
                    connection.Open();
                    
                    var result = await connection.QuerySingleOrDefaultAsync<string>(query, userId);
                    return result;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> InvalidatePasswordResetToken(AppUser entity)
        {
            try
            {
                var query = $"UPDATE [Users] SET [PasswordResetToken] = {string.Empty} WHERE [ContactId] = {entity.Id}";
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("TBSS")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(query, entity);
                    return result;
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task LoginWithSignInManager(string UserName, string Password, bool isPersistent, bool lockOutOnFailure)
        {
            try
            {
                await signInManager.SignOutAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error | {ex.InnerException}");
            }
        }

        public async Task LogoutWithSignInManager()
        {
            try
            {
                await signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error | {ex.InnerException}");
            }
        }

        public async Task<ResetPasswordResponse> ResetPassword(AppUser entity)
        {
            try
            {
                var result = await userManager.ResetPasswordAsync(entity, entity.PasswordResetToken, entity.Password);
                if (result.Succeeded)
                {
                    return new ResetPasswordResponse { Successful = true, Message = "Password reset successfully!" };
                }
                else
                {
                    List<string> errors = new();
                    foreach (var error in result.Errors)
                    {
                        errors.Add(error.Description);
                    }

                    return new ResetPasswordResponse { Successful = false, Message = "Password reset failed.", Errors = errors };
                }

            }
            catch (Exception )
            {

                throw ;
            }
        }

        public Task<int> UpdateAuthToken(AuthToken entity)
        {
            throw new NotImplementedException();
        }

    }
}
