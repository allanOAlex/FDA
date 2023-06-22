﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using TB.Domain.Models;
using TB.Shared.Requests.Employee;
using TB.Shared.Requests.User;
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

        public async Task<UpdateEmployeeSalaryResponse> MySQL_Dapper_UpdateEmployeeSalaryAsync(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest)
        {
            try
            {
                var employee = await unitOfWork.Employee.FindByCondition(e => e.Id == updateEmployeeSalaryRequest.Id);
                if (employee != null)
                {
                    var request = new MapperConfiguration(cfg => cfg.CreateMap<UpdateEmployeeSalaryRequest, Employee>());
                    var response = new MapperConfiguration(cfg => cfg.CreateMap<Employee, UpdateEmployeeSalaryResponse>());

                    IMapper requestMap = request.CreateMapper();
                    IMapper responseMap = response.CreateMapper();

                    var destination = requestMap.Map<UpdateEmployeeSalaryRequest, Employee>(updateEmployeeSalaryRequest);
                    destination.Salary = updateEmployeeSalaryRequest.NewSalary;
                    var result = await unitOfWork.Employee.MySQL_Dapper_SP_UpdateEmployeeSalaryAsync(destination);
                    var updateResponse = responseMap.Map<Employee, UpdateEmployeeSalaryResponse>(result);
                    updateResponse.OldSalary = employee.FirstOrDefault()!.Salary;
                    updateResponse.Successful = true;
                    updateResponse.Message = "Salary updated successfully!";

                    return updateResponse;
                }
                
                return new UpdateEmployeeSalaryResponse { Successful = false, Message = "Employee does not exist" };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<UpdateEmployeeSalaryResponse> MySQL_EFCore_UpdateEmployeeSalaryAsync(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest)
        {
            try
            {
                var request = new MapperConfiguration(cfg => cfg.CreateMap<UpdateEmployeeSalaryRequest, Employee>());
                var response = new MapperConfiguration(cfg => cfg.CreateMap<Employee, UpdateEmployeeSalaryResponse>());

                IMapper requestMap = request.CreateMapper();
                IMapper responseMap = response.CreateMapper();

                var destination = requestMap.Map<UpdateEmployeeSalaryRequest, Employee>(updateEmployeeSalaryRequest);
                destination.Salary = updateEmployeeSalaryRequest.NewSalary;
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<UpdateEmployeeSalaryResponse> UpdateEmployeeSalary(UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest)
        {
            try
            {
                var request = new MapperConfiguration(cfg => cfg.CreateMap<UpdateEmployeeSalaryRequest, Employee>());
                var response = new MapperConfiguration(cfg => cfg.CreateMap<Employee, UpdateEmployeeSalaryResponse>());

                IMapper requestMap = request.CreateMapper();
                IMapper responseMap = response.CreateMapper();

                var destination = requestMap.Map<UpdateEmployeeSalaryRequest, Employee>(updateEmployeeSalaryRequest);
                destination.Salary = updateEmployeeSalaryRequest.NewSalary;
                var result = await unitOfWork.Employee.Update(destination);

                try
                {
                    await unitOfWork.CompleteAsync();
                    bool Successful = true;
                    return Successful == true ? new UpdateEmployeeSalaryResponse() { Successful = true, Message = "Employee salary updated successfully!" } : new UpdateEmployeeSalaryResponse() { Successful = false, Message = "Error updating employee salary" };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }




            }
            catch (Exception)
            {

                throw;
            }

            throw new NotImplementedException();
        }






    }
}