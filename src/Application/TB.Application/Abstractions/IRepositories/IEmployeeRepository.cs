using TB.Domain.Models;
using TB.Shared.Dtos;
using TB.Shared.Responses.Employee;

namespace TB.Application.Abstractions.IRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<(Employee, int)> UpdatesEmployeeSalaryAsync(Employee employee);
        Task<Employee> UpdateEmployeeSalaryAsync(Employee employee);



    }
}
