﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using System.Linq.Expressions;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.MySQL.MySQL;
using TB.Shared.Dtos;
using TB.Shared.Responses.Employee;

namespace TB.Infrastructure.Implementations.Repositories
{
    /// <summary>
    /// Sealed class 
    /// </summary>
    internal sealed class EmployeeRepository : IBaseRepository<Employee>, IEmployeeRepository
    {
        private readonly IConfiguration configuration;
        private readonly MyDBContext context;

        public EmployeeRepository(IConfiguration Configuration, MyDBContext Context)
        {
            
            configuration = Configuration;
            context = Context;
        }

        public Task<Employee> Create(Employee entity)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> Delete(Employee entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Employee>> FindAll()
        {
            return await Task.FromResult(context.Employees!.OrderByDescending(e => e.Id).AsNoTracking());
        }

        public async Task<IQueryable<Employee>> FindByCondition(Expression<Func<Employee, bool>> expression)
        {
            return await Task.FromResult(context.Employees!.Where(expression).AsNoTracking());

        }

        public Task<Employee?> FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> Update(Employee entity)
        {
            throw new NotImplementedException();
        }


        public async Task<(Employee, int)> UpdatesEmployeeSalaryAsync(Employee employee)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(configuration.GetConnectionString("TBMS")))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@empID", employee.Id);
                    parameters.Add("@newSalary", employee.Salary);
                    parameters.Add("@oldSalary", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await connection.ExecuteAsync("UpdateEmployeeSalary", parameters, commandType: CommandType.StoredProcedure);

                    object oldSalaryObj = parameters.Get<object>("@oldSalary");
                    int oldSalary = (oldSalaryObj != DBNull.Value) ? Convert.ToInt32(oldSalaryObj) : 0;

                    if (!string.IsNullOrEmpty(oldSalary.ToString()))
                    {
                        return (employee, oldSalary);
                    }

                    throw new Exception("Error updating employee salary");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Employee> UpdateEmployeeSalaryAsync(Employee employee)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(configuration.GetConnectionString("TBMS")))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@empID", employee.Id);
                    parameters.Add("@newSalary", employee.Salary);
                    parameters.Add("@oldSalary", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("UpdateEmployeeSalary", parameters, commandType: CommandType.StoredProcedure);

                    object updatedOldSalary = parameters.Get<int>("@oldSalary");
                    int oldSalary = (updatedOldSalary != DBNull.Value) ? Convert.ToInt32(updatedOldSalary) : 0;

                    return oldSalary != 0 ? new Employee { Id = employee.Id, Salary = employee.Salary } : new Employee { Id = employee.Id, Salary = employee.Salary };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




    }
}
