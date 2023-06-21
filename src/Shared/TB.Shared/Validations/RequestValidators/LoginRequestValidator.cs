using FluentValidation;
using TB.Shared.Requests.Auth;

namespace TB.Shared.Validations.RequestValidators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            When(x => x != null, () => {

                RuleFor(x => x.UserName).NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches("^[a-zA-Z0-9]+$");


                RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} is required.")
                    .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters long.")
                    .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
                
            });

            
                
        }
    }
}
