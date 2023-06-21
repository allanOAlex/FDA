using FluentValidation;
using TB.Shared.Requests.Auth;

namespace TB.Shared.Validations.RequestValidators
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id cannot be null."); 
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be null."); 
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.PasswordConfirm).NotEmpty().WithMessage("Password confirmation is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        }
    }
}
