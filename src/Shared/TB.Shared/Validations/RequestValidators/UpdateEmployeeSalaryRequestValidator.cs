using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Requests.Auth;
using TB.Shared.Requests.Employee;

namespace TB.Shared.Validations.RequestValidators
{
    public class UpdateEmployeeSalaryRequestValidator : AbstractValidator<UpdateEmployeeSalaryRequest>
    {
        public UpdateEmployeeSalaryRequestValidator()
        {
            RuleFor(x => x.NewSalary).NotEmpty().WithMessage("New salary cannot be null.");
        }
    }
}
