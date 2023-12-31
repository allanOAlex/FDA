﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using MySqlConnector;
using System.Data;
using TB.Domain.Models;
using TB.Persistence.MySQL.MySQL;
using TB.Tests.NUnit.Abstraction.Tests;
using TB.Tests.NUnit.Infrastructure.Tests.Repository.Tests.MockRepositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TB.Tests.NUnit.Infrastructure.Tests.Repository.Tests.Tests
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        private MockEmployeeRepository_NoDataBase repository;

        private IConfiguration configuration;
        private string connectionString;


        [SetUp]
        public void Setup()
        {
            repository = new MockEmployeeRepository_NoDataBase();
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("TBMS")!;

        }

        [TearDown] 
        public void Teardown() 
        {
            var options = new DbContextOptionsBuilder<MyDBContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;

            using (var context = new MyDBContext(options))
            {
                var testEmployees = context.Employees!
                    .Where(e => e.Name!.Contains("Test"))
                    .ToListAsync();

                context.Employees!.RemoveRange(testEmployees.Result);
                context.SaveChangesAsync();
            }
        }  
        

        [Test]
        public async Task TestUpdatesEmployeeSalary()
        {
            var options = new DbContextOptionsBuilder<MyDBContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            using (var context = new MyDBContext(options))
            {
                var repository = new MockEmployeeRepository(configuration, context);

                int initialSalary = 50000;
                int updatedSalary = 60000;

                int lastId;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT MAX(Id) FROM Employees;";
                    var command = new MySqlCommand(query, connection);
                    var maxId = command.ExecuteScalar();
                    lastId = maxId != DBNull.Value ? Convert.ToInt32(maxId) : 0;
                }

                var newEmployee = new Employee
                {
                    Id = lastId + 1,
                    Name = string.Join(" ", "Test", lastId + 1),
                    Salary = initialSalary
                };

                context.Employees!.Add(newEmployee);
                await context.SaveChangesAsync();

                try
                {
                    // Act
                    var employee = new Employee
                    {
                        Id = lastId + 1,
                        Salary = updatedSalary
                    };

                    var result = await repository.TestUpdatesEmployeeSalary(employee, out int oldSalary);

                    // Assert
                    Assert.That(updatedSalary, Is.Not.EqualTo(oldSalary));
                    Assert.That(employee, Is.EqualTo(result));


                    // Optionally, assert that the salary in the database has been updated correctly
                    Assert.That(updatedSalary, Is.EqualTo(employee!.Salary));

                    // Clean up the test data (e.g., delete the test employee record)
                    var updatedEmployee = await context.Employees.FindAsync(employee.Id);
                    context.Employees.Remove(updatedEmployee!);
                    await context.SaveChangesAsync();

                    context.Entry(newEmployee).State = EntityState.Detached;
                }
                finally
                {
                    context.Dispose();
                }
            }

        }

        [Test]
        public async Task TestUpdatesEmployeeSalaryAsync_()
        {
            var options = new DbContextOptionsBuilder<MyDBContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            using (var context = new MyDBContext(options))
            {
                var repository = new MockEmployeeRepository(configuration, context);

                int initialSalary = 50000;
                int updatedSalary = 60000;

                int lastId;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT MAX(Id) FROM Employees;";
                    var command = new MySqlCommand(query, connection);
                    var maxId = command.ExecuteScalar();
                    lastId = maxId != DBNull.Value ? Convert.ToInt32(maxId) : 0;
                }

                var newEmployee = new Employee
                {
                    Id = lastId + 1,
                    Name = string.Join(" ", "Test", lastId + 1),
                    Salary = initialSalary
                };

                context.Employees!.Add(newEmployee);
                await context.SaveChangesAsync();

                try
                {
                    // Act
                    var employee = new Employee
                    {
                        Id = lastId + 1,
                        Salary = updatedSalary
                    };

                    //var result = await repository.TestUpdatesEmployeeSalary(employee, out int oldSalary);
                    (Employee returnEmployee, int oldSalary) = await repository.TestUpdatesEmployeeSalaryAsync_(employee);


                    // Assert
                    Assert.That(updatedSalary, Is.Not.EqualTo(oldSalary));
                    Assert.That(employee, Is.EqualTo(returnEmployee));


                    // Optionally, assert that the salary in the database has been updated correctly
                    Assert.That(updatedSalary, Is.EqualTo(employee!.Salary));

                    // Clean up the test data (e.g., delete the test employee record)
                    var updatedEmployee = await context.Employees.FindAsync(employee.Id);
                    context.Employees.Remove(updatedEmployee!);
                    await context.SaveChangesAsync();

                    context.Entry(newEmployee).State = EntityState.Detached;
                }
                finally
                {
                    context.Dispose();
                }
            }

        }

        [Test]
        public async Task TestUpdatesEmployeeSalaryAsync()
        {
            var options = new DbContextOptionsBuilder<MyDBContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            using (var context = new MyDBContext(options))
            {
                var repository = new MockEmployeeRepository(configuration, context);

                int initialSalary = 50000;
                int updatedSalary = 60000;

                int lastId;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT MAX(Id) FROM Employees;";
                    lastId = await connection.ExecuteScalarAsync<int?>(query) ?? 0;
                }

                var newEmployee = new Employee
                {
                    Id = lastId + 1,
                    Name = string.Join(" ","Test", lastId + 1),
                    Salary = initialSalary
                };

                try
                {
                    context.Employees!.Add(newEmployee);
                    await context.SaveChangesAsync();

                    // Act
                    var employee = new Employee
                    {
                        Id = lastId + 1,
                        Salary = updatedSalary
                    };

                    var result = await repository.TestUpdatesEmployeeSalaryAsync(employee);
                    await context.SaveChangesAsync();


                    // Assert
                    Assert.That(updatedSalary, Is.Not.EqualTo(result.OldSalary));


                    // Clean up the test data (e.g., delete the test employee record)
                    var updatedEmployee = await context.Employees.FindAsync(employee.Id);
                    context.Employees.Remove(updatedEmployee!);
                    await context.SaveChangesAsync();

                    context.Entry(newEmployee).State = EntityState.Detached;

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {

                }

            }

            
        }

        [Test]
        public async Task TestUpdatesEmployeeSalary_ValidInput_ReturnsEmployeeIdAndOldSalary()
        {
            // Arrange
            int oldSalary = 5000;

            var employee = new Employee
            {
                Id = 1,
                Name = "Test",  
                Salary = 7000
            };

            // Act
            var result = await repository.TestUpdatesEmployeeSalary(employee);

            // Assert
            Assert.That(result.Salary, Is.Not.EqualTo(oldSalary));
            Assert.That(result.Salary, Is.EqualTo(7000));


        }





    }
}
