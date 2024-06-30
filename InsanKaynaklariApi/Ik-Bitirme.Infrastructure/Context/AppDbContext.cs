using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Infrastructure.EntityTypeConfigs;
using Ik_Bitirme.Infrastructure.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdvanceRequest> AdvanceRequests { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ExpenseRequest> ExpenseRequests { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ik-server.database.windows.net;Database=InsanKaynaklariYonetim;User Id=admin1;Password=Bilge123.");
        }
           
        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region SeedDataRoles
            builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "Admin",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = new Guid().ToString(),
            },
            new IdentityRole
            {
                Id = "Employee",
                Name = "Employee",
                NormalizedName = "EMPLOYEE",
                ConcurrencyStamp = new Guid().ToString(),
            },
            new IdentityRole
            {
                Id = "Director",
                Name = "Director",
                NormalizedName = "DIRECTOR",
                ConcurrencyStamp = new Guid().ToString(),
            });
            #endregion

            #region Admin Seed Data
            var adminUser = new AppUser
            {
                Id = "1",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                FirstName = "admin",
                LastName = "admin",
                CreateDate = DateTime.Now,
                //ImagePath = $"/images/01-admin.jpg",
                //ImageData = "abc",
                Status = Domain.Enums.Status.Active,
                EmailConfirmed = true,
                Address = "abc",
                BirthDate = DateTime.Now,
                BirthPlace = "aaa",
                TcIdentity = "123123",
                HireDate = DateTime.Now,
                PhoneNumber = "1234567890",
                JobId = 1,
                DepartmentId = 1,
                Salary = 123
            };

            var passwordHasher = new PasswordHasher<AppUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "1234");

            builder.Entity<AppUser>().HasData(adminUser);
            builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = "Admin",
                        UserId = adminUser.Id
                    });
            #endregion

            builder.Seed();

            // En Altta
            base.OnModelCreating(builder);
        }
    }
}
