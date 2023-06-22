using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.MySQL.MySQL;
using TB.Shared.Requests.Employee;
using TB.Shared.Responses.Employee;

namespace TB.Infrastructure.Implementations.Repositories
{
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

        public Task<IQueryable<Employee>> FindByCondition(Expression<Func<Employee, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Employee?> FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> MySQL_Dapper_UpdateEmployeeSalaryAsync(Employee employee)
        {
            try
            {
                var query = $"UPDATE [Employees] SET [Salary] = {employee.Salary} WHERE [Id] = {employee.Id}";
                using (IDbConnection connection = new MySqlConnection(configuration.GetConnectionString("TBMS")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(query, employee);
                    if (result >= 1)
                    {
                        return employee;
                    }
                    throw new Exception("Error updating employee salary");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Employee> MySQL_Dapper_SP_UpdateEmployeeSalaryAsync(Employee employee)
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

                    var oldSalary = parameters.Get<int>("@oldSalary");

                    if (result >= 1)
                    {
                        return employee;
                    }

                    throw new Exception("Error updating employee salary");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<Employee> MySQL_EFCore_UpdateEmployeeSalaryAsync(Employee employee)
        {
            throw new NotImplementedException();
        }


        public Task<Employee> Update(Employee entity)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateEmployeeSalary(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
