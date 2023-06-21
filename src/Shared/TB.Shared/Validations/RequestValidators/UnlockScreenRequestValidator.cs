using FluentValidation;
using TB.Shared.Requests.Auth;

namespace TB.Shared.Validations.RequestValidators
{
    public class UnlockScreenRequestValidator : AbstractValidator<UnlockScreenRequest>
    {
        public UnlockScreenRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be null / empty.").MinimumLength(8);
        }
    }
}
