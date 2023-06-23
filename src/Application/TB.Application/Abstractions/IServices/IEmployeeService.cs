using TB.Shared.Requests.Employee;
using TB.Shared.Responses.Employee;

namespace TB.Application.Abstractions.IServices
{
    public interface IEmployeeService
    {
        Task<List<GetAllEmployeesResponse>> FindAll();
        Task<UpdateEmployeeSalaryResponse> MySQL_Dapper_UpdateEmployeeSalaryAsync(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest); 
    }
}
