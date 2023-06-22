using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Employee
{
    public record UpdateEmployeeSalaryRequest : Request
    {
        [Required(ErrorMessage = "Salary is required")]
        public int NewSalary { get; set; }
    }
}
