using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Employee
{
    public record UpdateEmployeeSalaryResponse : Response
    {
        public string? Name { get; set; }
        public int OldSalary { get; set; }
        public int NewSalary { get; set; } 
    }
}
