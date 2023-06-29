using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using TB.Domain.Models;
using TB.Infrastructure.Implementations.Interfaces;
using TB.Shared.Requests.Employee;
using TB.Shared.Responses.Employee;
using TB.Shared.Responses.User;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeeService(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        public async Task<List<GetAllEmployeesResponse>> FindAll()
        {
            try
            {
                List<GetAllEmployeesResponse> empList = new();
                var employees = await unitOfWork.Employee.FindAll();
                if (employees.Any())
                {
                    foreach (var emp in employees)
                    {
                        var listItem = new GetAllEmployeesResponse
                        {
                            Id = emp.Id,
                            Name = emp.Name,
                            Salary = emp.Salary,
                            
                        };

                        empList.Add(listItem);
                    }

                }

                return empList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UpdateEmployeeSalaryResponse> UpdateEmployeeSalaryAsync(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest)
        {
            try
            {
                var employee = await unitOfWork.Employee.FindByCondition(e => e.Id == updateEmployeeSalaryRequest.EmployeeId);
                if (employee != null)
                {
                    var request = new MapperConfiguration(cfg => cfg.CreateMap<UpdateEmployeeSalaryRequest, Employee>());
                    var response = new MapperConfiguration(cfg => cfg.CreateMap<Employee, UpdateEmployeeSalaryResponse>());

                    IMapper requestMap = request.CreateMapper();
                    IMapper responseMap = response.CreateMapper();

                    var destination = requestMap.Map<UpdateEmployeeSalaryRequest, Employee>(updateEmployeeSalaryRequest);

                    (Employee returnEmployee, int oldSalary) = await unitOfWork.Employee.UpdatesEmployeeSalaryAsync(destination);
                    var updatedEmployee = responseMap.Map<Employee, UpdateEmployeeSalaryResponse>(returnEmployee);

                    try
                    {
                        await unitOfWork.CompleteAsync();

                        updatedEmployee.OldSalary = oldSalary;
                        updatedEmployee.NewSalary = employee.FirstOrDefault()!.Salary;
                        updatedEmployee.Successful = true;
                        updatedEmployee.Message = "Salary updated successfully!";

                        return updatedEmployee;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        throw ex;
                    }

                    
                }
                
                return new UpdateEmployeeSalaryResponse { Successful = false, Message = "Employee does not exist" };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UpdateEmployeeSalaryResponse> UpdateEmployeeSalaryAsync_(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest)
        {
            try
            {
                var employee = await unitOfWork.Employee.FindByCondition(e => e.Id == updateEmployeeSalaryRequest.EmployeeId);
                if (employee != null)
                {
                    var request = new MapperConfiguration(cfg => cfg.CreateMap<UpdateEmployeeSalaryRequest, Employee>());
                    var response = new MapperConfiguration(cfg => cfg.CreateMap<Employee, UpdateEmployeeSalaryResponse>());

                    IMapper requestMap = request.CreateMapper();
                    IMapper responseMap = response.CreateMapper();

                    var destination = requestMap.Map<UpdateEmployeeSalaryRequest, Employee>(updateEmployeeSalaryRequest);
                    var result = await unitOfWork.Employee.UpdateEmployeeSalaryAsync(destination);
                    var updateResponse = responseMap.Map<Employee, UpdateEmployeeSalaryResponse>(result);

                    try
                    {
                        await unitOfWork.CompleteAsync();

                        updateResponse.OldSalary = employee.FirstOrDefault()!.Salary;
                        updateResponse.NewSalary = destination.Salary;
                        updateResponse.Successful = true;
                        updateResponse.Message = "Salary updated successfully!";

                        return updateResponse;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        throw ex;
                    }


                }

                return new UpdateEmployeeSalaryResponse { Successful = false, Message = "Employee does not exist" };

            }
            catch (Exception)
            {

                throw;
            }
        }




    }



}
