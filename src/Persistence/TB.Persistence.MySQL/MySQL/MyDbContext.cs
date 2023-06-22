using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Domain.Models;

namespace TB.Persistence.MySQL.MySQL
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {

        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }


        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Dividend>? Dividends { get; set; }
        public DbSet<Earning>? Earnings { get; set; }
        public DbSet<StockPrice>? StockPrices { get; set; }


    }
}
