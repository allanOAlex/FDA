using TB.Domain.Models;

namespace TB.Application.Abstractions.IRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee> MySQL_Dapper_SP_UpdateEmployeeSalaryAsync(Employee employee, out int oldSalary);
        Task<(Employee, int)> UpdateEmployeeSalaryAsync(Employee employee);



    }
}
