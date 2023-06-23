using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Employee
{
    public record GetAllEmployeesResponse : Response
    {
        public string? Name { get; set; }
        public int Salary { get; set; }
    }
}
