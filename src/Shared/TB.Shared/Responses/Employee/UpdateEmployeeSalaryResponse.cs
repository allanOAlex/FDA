using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
