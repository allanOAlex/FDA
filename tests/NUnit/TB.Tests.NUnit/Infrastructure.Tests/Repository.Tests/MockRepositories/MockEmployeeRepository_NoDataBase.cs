using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Domain.Models;

namespace TB.Tests.NUnit.Infrastructure.Tests.Repository.Tests.MockRepositories
{
    internal sealed class MockEmployeeRepository_NoDataBase
    {
        public async Task<Employee> TestUpdatesEmployeeSalary(Employee employee)
        {
            return new Employee
            {
                Id = employee.Id,
                Salary = employee.Salary,
            };
        }
    }
}
