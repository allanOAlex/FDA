using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using System.Linq.Expressions;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.MySQL.MySQL;
using TB.Shared.Dtos;

namespace TB.Tests.NUnit.Infrastructure.Tests.Repository.Tests.MockRepositories
{
    internal sealed class MockEmployeeRepository
    {
        private readonly IConfiguration configuration;
        private readonly MyDBContext context;

        public MockEmployeeRepository(IConfiguration Configuration, MyDBContext Context)
        {
            configuration = Configuration;
            context = Context;
        }


        public Task<Employee> MySQL_Dapper_SP_UpdateEmployeeSalaryAsync(Employee employee, out int oldSalary)
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

                    var result = connection.ExecuteAsync("UpdateEmployeeSalary", parameters, commandType: CommandType.StoredProcedure);

                    object oldSalaryObj = parameters.Get<object>("@oldSalary");
                    oldSalary = (oldSalaryObj != DBNull.Value) ? Convert.ToInt32(oldSalaryObj) : 0;

                    if (!string.IsNullOrEmpty(oldSalary.ToString()))
                    {
                        return Task.FromResult(employee);
                    }

                    throw new Exception("Error updating employee salary");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SPEmployeeDto> UpdateEmployeeSalaryAsync(Employee employee)
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

                    if (oldSalary != 0)
                    {
                        return new SPEmployeeDto(oldSalary,employee.Id,employee.Name,employee.Salary);
                    }

                    throw new Exception("Error updating employee salary");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
