using System.ComponentModel.DataAnnotations;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Employee
{
    public record UpdateEmployeeSalaryRequest : Request
    {
        [Required(ErrorMessage = "Salary is required")]
        public int EmployeeId { get; set; }
        public int Salary { get; set; }
    }
}
