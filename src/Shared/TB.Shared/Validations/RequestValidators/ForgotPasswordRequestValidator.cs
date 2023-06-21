using FluentValidation;
using TB.Shared.Requests.Auth;

namespace TB.Shared.Validations.RequestValidators
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be null.");
        }
    }
}
