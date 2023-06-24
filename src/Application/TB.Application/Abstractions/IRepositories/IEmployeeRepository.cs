using TB.Domain.Models;
using TB.Shared.Dtos;

namespace TB.Application.Abstractions.IRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee> UpdateEmployeeSalaryAsync(Employee employee, out int oldSalary);
        Task<UpdateEmployeeDto> UpdateEmployeeSalaryAsync(Employee employee);



    }
}
