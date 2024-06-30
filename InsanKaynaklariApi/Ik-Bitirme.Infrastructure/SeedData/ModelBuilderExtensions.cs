using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Infrastructure.SeedData
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, Name = "Information Technology", CreateDate = DateTime.Now },
                new Department { DepartmentId = 2, Name = "Human Resources", CreateDate = DateTime.Now },
                new Department { DepartmentId = 3, Name = "Accounting", CreateDate = DateTime.Now },
                new Department { DepartmentId = 4, Name = "Marketing", CreateDate = DateTime.Now },
                new Department { DepartmentId = 5, Name = "Sales", CreateDate = DateTime.Now }
            );

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    CompanyId = 1,
                    Name = "Firma1",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,
                    Email = "abc@company.com",
                    Address = "Ankara",
                    PhoneNumber = "123456789",
                    MersisNo = "123456789",
                    ContractStartDate = DateTime.Now,
                    ContractEndDate = DateTime.Now,
                    Title = "ltd",
                    TaxAdministration = "vergiDariresi",
                    TaxNo = "123456789",
                    YearOfEstablishment = DateTime.Now
                },
                new Company
                {
                    CompanyId = 2,
                    Name = "Firma2",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,
                    Email = "scd@company.com",
                    Address = "Ankara",
                    PhoneNumber = "123456789",
                    MersisNo = "123456789",
                    ContractStartDate = DateTime.Now,
                    ContractEndDate = DateTime.Now,
                    Title = "ltd",
                    TaxAdministration = "vergiDariresi",
                    TaxNo = "123456789",
                    YearOfEstablishment = DateTime.Now
                },
                new Company
                {
                    CompanyId = 3,
                    Name = "Firma3",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,
                    Email = "dxs@company.com",
                    Address = "Ankara",
                    PhoneNumber = "123456789",
                    MersisNo = "123456789",
                    ContractStartDate = DateTime.Now,
                    ContractEndDate = DateTime.Now,
                    Title = "ltd",
                    TaxAdministration = "vergiDariresi",
                    TaxNo = "123456789",
                    YearOfEstablishment = DateTime.Now
                },
                new Company
                {
                    CompanyId = 4,
                    Name = "Firma4",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,
                    Email = "qwe@company.com",
                    Address = "İstanbul",
                    PhoneNumber = "123456789",
                    MersisNo = "123456789",
                    ContractStartDate = DateTime.Now,
                    ContractEndDate = DateTime.Now,
                    Title = "ltd",
                    TaxAdministration = "vergiDariresi",
                    TaxNo = "123456789",
                    YearOfEstablishment = DateTime.Now
                },
                new Company
                {
                    CompanyId = 5,
                    Name = "Firma5",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,
                    Email = "rxa@company.com",
                    Address = "İstanbul",
                    PhoneNumber = "123456789",
                    MersisNo = "123456789",
                    ContractStartDate = DateTime.Now,
                    ContractEndDate = DateTime.Now,
                    Title = "ltd",
                    TaxAdministration = "vergiDariresi",
                    TaxNo = "123456789",
                    YearOfEstablishment = DateTime.Now
                }
            );

            modelBuilder.Entity<Job>().HasData(
    new Job { Id = 1, Name = "Software Developer", Description = "Developing software applications for various platforms.", CreateDate = DateTime.Now, Status = Status.Active },
    new Job { Id = 2, Name = "Data Analyst", Description = "Analyzing and interpreting complex data sets to provide insights.", CreateDate = DateTime.Now, Status = Status.Active },
    new Job { Id = 3, Name = "Project Manager", Description = "Overseeing project timelines, resources, and deliverables.", CreateDate = DateTime.Now, Status = Status.Active },
    new Job { Id = 4, Name = "Marketing Specialist", Description = "Creating and executing marketing campaigns to promote products.", CreateDate = DateTime.Now, Status = Status.Active },
    new Job { Id = 5, Name = "Customer Support Representative", Description = "Assisting customers with product inquiries and issue resolution.", CreateDate = DateTime.Now, Status = Status.Active }
);


            var passwordHasher = new PasswordHasher<Employee>();
            var hashedPassword = passwordHasher.HashPassword(null, "123");

            var employee = new Employee
            {
                Id = "2",
                UserName = "ahmet",
                NormalizedUserName = "AHMET",
                Email = "ahmet@ahmet.com",
                NormalizedEmail = "AHMET@EXAMPLE.COM",
                FirstName = "sagasg",
                LastName = "asgsag",
                CreateDate = DateTime.Now,
                //ImagePath = "abc",
                Status = Domain.Enums.Status.Active,
                EmailConfirmed = true,
                Address = "abc",
                BirthDate = DateTime.Now,
                BirthPlace = "aaa",
                TcIdentity = "123123",
                HireDate = DateTime.Now,
                PhoneNumber = "1234567890",
                Salary = 123,
                PasswordHash = hashedPassword,
                DepartmentId = 1,
                CompanyId = 1,
                Expenses = null,
                Leaves = null,
                Advances = null,
                JobId = 1
            };

            modelBuilder.Entity<Employee>().HasData(employee);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "Employee",
                    UserId = employee.Id
                });


            var passwordHasherDirector = new PasswordHasher<Director>();
            var hashedPasswordDirector = passwordHasherDirector.HashPassword(null, "123");

            var director = new Director
            {
                Id = "3",
                UserName = "director",
                NormalizedUserName = "Direktör",
                Email = "director@director.com",
                NormalizedEmail = "director@EXAMPLE.COM",
                FirstName = "Mr Direktör",
                LastName = "Direktor Soyisim",
                CreateDate = DateTime.Now,
                //ImagePath = "abc",
                Status = Domain.Enums.Status.Active,
                EmailConfirmed = true,
                Address = "abc",
                BirthDate = DateTime.Now,
                BirthPlace = "aaa",
                TcIdentity = "123123",
                HireDate = DateTime.Now,
                PhoneNumber = "1234567890",
                Salary = 123,
                PasswordHash = hashedPasswordDirector,
                DepartmentId = 1,
                CompanyId = 1,                
                JobId = 1
            };

            modelBuilder.Entity<Director>().HasData(director);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = "Director",
                        UserId = director.Id
                    });


        }
    }
}
