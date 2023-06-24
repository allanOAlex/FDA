using Microsoft.EntityFrameworkCore;
using TB.Domain.Models;

namespace TB.Persistence.MySQL.MySQL
{
    public class MyDBContext : DbContext
    {
        public MyDBContext()
        {

        }

        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);


        }

        protected void SeedData(ModelBuilder builder)
        {
            builder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "John Doe", Salary = 5000 },
                new Employee { Id = 2, Name = "Jane Smith", Salary = 6000 }
            );
        }


        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Dividend>? Dividends { get; set; }
        public DbSet<Earning>? Earnings { get; set; }
        public DbSet<StockPrice>? StockPrices { get; set; }


    }
}
