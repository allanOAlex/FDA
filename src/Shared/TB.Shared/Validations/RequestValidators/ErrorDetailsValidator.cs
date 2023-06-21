using FluentValidation;
using TB.Shared.Dtos;

namespace TB.Shared.Validations.RequestValidators
{
    public class ErrorDetailsValidator : AbstractValidator<ErrorDetails>
    {
        public ErrorDetailsValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            When(x => x != null, () => {

                RuleFor(x => x.ExceptionTypeName).NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches("^[a-zA-Z0-9]+$");

                RuleFor(x => x.Source).NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches("^[a-zA-Z0-9]+$");

                RuleFor(x => x.Message).NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches("^[a-zA-Z0-9]+$");

                RuleFor(x => x.StatusCode.ToString()).NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches("^[0-9]+$");



            });
        }
    }
}
