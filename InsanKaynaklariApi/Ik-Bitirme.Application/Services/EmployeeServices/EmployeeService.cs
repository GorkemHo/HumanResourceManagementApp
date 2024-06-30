using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.AdminDtos;
using Ik_Bitirme.Application.Models.DTos.EmployeeDtos;
using Ik_Bitirme.Application.Models.VMs.UserVMs;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Domain.IRepositories;
using Ik_Bitirme.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICompanyRepo _companyRepository;
        private readonly IDepartmentRepo _departmentRepository;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly SignInManager<AppUser> _signInManager;


        public EmployeeService(IDepartmentRepo departmentRepository, ICompanyRepo companyRepository, IMapper mapper, UserManager<AppUser> userManager, IEmployeeRepo employeeRepo, SignInManager<AppUser> signInManager)
        {
            _departmentRepository = departmentRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _userManager = userManager;
            _employeeRepo = employeeRepo;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Register(RegisterEmployeeDto model)
        {
            var user = _mapper.Map<Employee>(model);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "EMPLOYEE");
            }
            return result;
        }

        public async Task<bool> UpdateEmployeeStatus(string userName, string status)
        {
            var user = await _employeeRepo.GetDefault(x => x.UserName.Equals(userName));

            if (user == null)
            {
                return false;
            }

            if (status == "Active")
            {
                user.Status = Status.Active;
            }
            else
            {
                user.Status = Status.Passive;
            }

            await _employeeRepo.UpdateAsync(user);
            return true;
        }
        public async Task UpdateEmployee(UpdateEmployeeDto model)
        {
            var user = await _employeeRepo.GetDefault(x => x.Id.Equals(model.Id)) ?? throw new Exception("User not found");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.MiddleName = model.MiddleName;
            user.SecondLastName = model.SecondLastName;
            user.BirthDate = model.BirthDate;
            user.TerminationDate = model.TerminationDate;
            user.BirthPlace = model.BirthPlace;
            user.TcIdentity = model.TcIdentity;
            user.HireDate = model.HireDate;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.Salary = model.Salary;
            user.JobId = model.JobId;
            user.DepartmentId = model.DepartmentId;
            user.CompanyId = model.CompanyId;

            if (model.ImageData != null)
            {
                user.ImageData = model.ImageData;
            }

            if (!string.IsNullOrEmpty(model.UserName))
            {
                var isUserNameExist = await _userManager.FindByNameAsync(model.UserName);

                if (isUserNameExist == null)
                {
                    await _userManager.SetUserNameAsync(user, model.UserName);
                    await _signInManager.SignInAsync(user, false);
                }
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                var isUserEmailExist = await _userManager.FindByEmailAsync(model.Email.ToUpper());

                if (isUserEmailExist == null)
                {
                    await _userManager.SetEmailAsync(user, model.Email);
                }
            }
            await _employeeRepo.UpdateAsync(user);
        }

        public async Task<EmployeeDto> GetByUserName(string userName)
        {
            var user = await _employeeRepo.GetFilteredFirstOrDefault(
                            select: x => _mapper.Map<EmployeeDto>(x),
                            where: x => x.UserName.Equals(userName) && x.Status != Status.Passive);

            return user;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployees()
        {
            var users = await _userManager.GetUsersInRoleAsync("Employee");
            var userDtos = new List<EmployeeDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var employeeDto = _mapper.Map<EmployeeDto>(user);
                employeeDto.Role = roles.FirstOrDefault();
                employeeDto.Email = user.Email;
                userDtos.Add(employeeDto);
            }
            return userDtos;
        }

        public async Task<bool> CreateEmployee(CreateEmployeeDto model)
        {
            var user = _mapper.Map<Employee>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");
                return true;    
            }
            return false;
        }

        public async Task<bool> DeleteEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.TerminationDate = DateTime.Now;
            user.Status = Status.Passive;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<List<Employee>> GetAllByCompany(int id)
        {
            List<Employee> result = new List<Employee>();
            List<Company> companies = await _companyRepository.GetDefaults(x => x.Status != Status.Passive);
            List<Department> departments = await _departmentRepository.GetDefaults(x => x.Status != Status.Passive);

            var employeeList = await _userManager.GetUsersInRoleAsync("Employee");

            var list = _mapper.Map<List<Employee>>(employeeList);

            result = list.Where(x => x.CompanyId == id && x.Status != Status.Passive).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                result[i].Company = companies.Find(x => x.CompanyId == list[i].CompanyId);
                result[i].Department = departments.Find(x => x.DepartmentId == list[i].DepartmentId);
            }
            return result;
        }
    }
}
