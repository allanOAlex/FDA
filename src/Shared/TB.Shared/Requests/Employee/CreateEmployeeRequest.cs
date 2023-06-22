using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Employee
{
    public record CreateEmployeeRequest : Request
    {
        public string? Name { get; set; }
        public int Salary { get; set; }
    }
}
