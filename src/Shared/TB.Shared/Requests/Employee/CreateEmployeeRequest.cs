using System.ComponentModel.DataAnnotations;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Employee
{
    public record CreateEmployeeRequest : Request
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        public int Salary { get; set; }
    }
}
