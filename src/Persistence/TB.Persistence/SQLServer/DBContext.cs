using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using TB.Domain.Models;

namespace TB.Persistence.SQLServer
{

    public class DBContext : IdentityDbContext<AppUser, AppUserRole, int>
    {

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DBContext()
        {
                
        }

        public class DBContextFactory : IDesignTimeDbContextFactory<DBContext>
        {
            DBContext IDesignTimeDbContextFactory<DBContext>.CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

                var connectionString = configuration.GetConnectionString("TB");

                var optionsBuilder = new DbContextOptionsBuilder<DBContext>();
                optionsBuilder.UseSqlServer(connectionString!);

                return new DBContext(optionsBuilder.Options);
            }
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            RenameIdentityTables(builder);
            ConfigureModels(builder);
            
        }

        protected void ConfigureModels(ModelBuilder modelBuilder)
        {
            
        }

        protected void RenameIdentityTables(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //create users
            AppUser appUser1 = new();
            AppUser appUser2 = new();

            AppUser[] appUser = new AppUser[]
            {
                appUser1 = new()
                {
                    Id = 1,
                    UserName = "allanOAlex",
                    NormalizedUserName = "ALLANOALEX",
                    FirstName = "Allan",
                    LastName = "Alex",
                    OtherNames = string.Empty,
                    Email = "allan.alex0803@gmail.com",
                    NormalizedEmail = "ALLAN.ALEX0803@GMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumber = string.Empty,
                    PhoneNumberConfirmed = false,
                    CreatedOn = DateTime.Now,
                    SecurityStamp = Guid.NewGuid().ToString(),

                },

                appUser2 = new()
                {
                    Id = 2,
                    UserName = "allanOdhiambo",
                    NormalizedUserName = "ALLANODHIAMBO",
                    FirstName = "Allan",
                    LastName = "Odhiambo",
                    OtherNames = string.Empty,
                    Email = "aamodhiambo@gmail.com",
                    NormalizedEmail = "AAMODHIAMBO@GMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumber = string.Empty,
                    PhoneNumberConfirmed = false,
                    CreatedOn = DateTime.Now,
                    SecurityStamp = Guid.NewGuid().ToString(),

                },
            };

            //set user passwords
            PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();
            appUser1.PasswordHash = ph.HashPassword(appUser1, "pa$5@Auth");
            appUser2.PasswordHash = ph.HashPassword(appUser2, "pa$5@Ally");

            //seed roles
            builder.Entity<AppUserRole>(entity =>
            {
                entity.ToTable(name: "Roles").HasData(
                    new AppUserRole
                    {
                        Id = 1,
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN"
                    },
                    new AppUserRole
                    {
                        Id = 2,
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new AppUserRole
                    {
                        Id = 3,
                        Name = "User",
                        NormalizedName = "USER"

                    });
            });

            //seed user
            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "Users").HasData(appUser);
            });

            //set roles
            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles").HasData(
                    new IdentityUserRole<int>
                    {
                        RoleId = 1,
                        UserId = 1
                    },
                    new IdentityUserRole<int>
                    {
                        RoleId = 2,
                        UserId = 2
                    });
            });

            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens");
            });



        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }


        public DbSet<AppUser>? AppUsers { get; set; }
        public DbSet<AppUserLogin>? AppUserLogins { get; set; }
        public DbSet<AppUserRole>? AppUserRoles { get; set; }
        public DbSet<AppUserToken>? AppUserTokens { get; set; }
        public DbSet<AuthToken>? AuthTokens { get; set; }
        public DbSet<StockPrice>? FinancialData { get; set; }


    }

}
