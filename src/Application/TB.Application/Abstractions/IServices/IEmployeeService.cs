using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Domain.Models;
using TB.Shared.Requests.Employee;
using TB.Shared.Responses.Employee;
using TB.Shared.Responses.User;

namespace TB.Application.Abstractions.IServices
{
    public interface IEmployeeService
    {
        Task<List<GetAllEmployeesResponse>> FindAll();
        Task<UpdateEmployeeSalaryResponse> UpdateEmployeeSalary(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest);
        Task<UpdateEmployeeSalaryResponse> MySQL_EFCore_UpdateEmployeeSalaryAsync(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest); 
        Task<UpdateEmployeeSalaryResponse> MySQL_Dapper_UpdateEmployeeSalaryAsync(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest); 
    }
}
