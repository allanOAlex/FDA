using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Domain.Models;
using TB.Shared.Requests.Employee;
using TB.Shared.Responses.Employee;

namespace TB.Application.Abstractions.IRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee> MySQL_Dapper_UpdateEmployeeSalaryAsync(Employee employee);
        Task<Employee> MySQL_Dapper_SP_UpdateEmployeeSalaryAsync(Employee employee);

        Task<Employee> MySQL_EFCore_UpdateEmployeeSalaryAsync(Employee employee);
        Task<Employee> UpdateEmployeeSalary(Employee employee);


    }
}
