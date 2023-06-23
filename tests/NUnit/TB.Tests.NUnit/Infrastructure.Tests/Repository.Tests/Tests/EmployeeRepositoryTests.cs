using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using MySqlConnector;
using System.Data;
using TB.Domain.Models;
using TB.Persistence.MySQL.MySQL;
using TB.Tests.NUnit.Infrastructure.Tests.Repository.Tests.MockRepositories;

namespace TB.Tests.NUnit.Infrastructure.Tests.Repository.Tests.Tests
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        private Mock<IConfiguration> mockConfiguration;
        private IConfiguration configuration;
        private Mock<MyDBContext> mockContext;
        private Mock<IDbConnection> mockConnection;

        private string connectionString;


        [SetUp]
        public void Setup()
        {
            mockConfiguration = new Mock<IConfiguration>();
            mockContext = new Mock<MyDBContext>();
            mockConnection = new Mock<IDbConnection>();

            // Build the configuration
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("TBMS")!;

        }

        [TearDown] 
        public void Teardown() 
        {
            mockContext.Object.Dispose();
            mockContext.Setup(c => c.Dispose()).Verifiable();
            //mockContext.Verify();

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

                    var result = await repository.MySQL_Dapper_SP_UpdateEmployeeSalaryAsync(employee, out int oldSalary);

                    // Assert
                    Assert.That(updatedSalary, Is.Not.EqualTo(oldSalary));
                    Assert.That(employee, Is.EqualTo(result));

                    // Optionally, assert that the salary in the database has been updated correctly
                    var updatedEmployee = await context.Employees.FindAsync(employee.Id);
                    Assert.That(updatedSalary, Is.EqualTo(updatedEmployee!.Salary));

                    // Clean up the test data (e.g., delete the test employee record)
                    context.Employees.Remove(updatedEmployee);
                    await context.SaveChangesAsync();

                    context.Entry(newEmployee).State = EntityState.Detached;
                }
                finally
                {
                    context.Dispose();
                }
            }

            //mockContext.Verify();
        }

        [Test]
        public async Task TestUpdatesEmployeeSalary_RollBack()
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

                // Start the transaction
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
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

                        var res = await repository.UpdateEmployeeSalaryAsync(employee);

                        // Assert
                        Assert.That(updatedSalary, Is.Not.EqualTo(res.OldSalary));
                        //Assert.That(employee, Is.EqualTo(res));

                        // Optionally, assert that the salary in the database has been updated correctly
                        var updatedEmployee = await context.Employees.FindAsync(employee.Id);
                        Assert.That(updatedSalary, Is.EqualTo(updatedEmployee!.Salary));

                        // Roll back the transaction
                        await transaction.RollbackAsync();
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            }

            
        }






    }
}
