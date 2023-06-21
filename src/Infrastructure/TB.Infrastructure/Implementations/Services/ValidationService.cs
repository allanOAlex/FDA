using TB.Application.Abstractions.IServices;
using TB.Shared.Requests.Auth;
using TB.Shared.Validations.RequestValidators;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class ValidationService : IValidationService
	{
        public async Task<bool> ValidateAsync(dynamic request)
        {
            try
			{
                if (request = typeof(LoginRequest))
                {
                    var validator = new LoginRequestValidator();
                    var validationResults = await validator.ValidateAsync(request!);
                    if (!validationResults.IsValid)
                    {
                        return false;
                    }

                }

                return true;
            }
			catch (Exception)
			{

				throw;
			}
		}



        

    }
}
