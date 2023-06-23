using FluentValidation;
using TB.Shared.Requests.Employee;

namespace TB.Shared.Validations.RequestValidators
{
    public class UpdateEmployeeSalaryRequestValidator : AbstractValidator<UpdateEmployeeSalaryRequest>
    {
        public UpdateEmployeeSalaryRequestValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Employee Id cannot be null.");
            RuleFor(x => x.Salary).NotEmpty().WithMessage("New salary cannot be null.");
        }
    }
}
