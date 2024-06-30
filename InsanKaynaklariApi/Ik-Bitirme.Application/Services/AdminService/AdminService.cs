using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.AdminDtos;
using Ik_Bitirme.Application.Models.DTos.CompanyDto;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IAppUserRepo _userRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IAdminRepo _adminRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDirectorRepo _directorRepo;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public AdminService(IAppUserRepo userRepo, IEmployeeRepo employeeRepo, IAdminRepo adminRepo, UserManager<AppUser> userManager, IDirectorRepo directorRepo, SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _userRepo = userRepo;
            _employeeRepo = employeeRepo;
            _adminRepo = adminRepo;
            _userManager = userManager;
            _directorRepo = directorRepo;
            _signInManager = signInManager;
            _mapper = mapper;
        }



        public async Task<IdentityResult> Register(RegisterAdminDto model)
        {
            var user = _mapper.Map<Admin>(model);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "ADMIN");
                await _signInManager.SignInAsync(user, false);
            }
            return result;
        }


        public async Task<bool> UpdateAdminStatus(string userName, string status)
        {
            var user = await _adminRepo.GetDefault(x => x.UserName.Equals(userName));

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

            await _adminRepo.UpdateAsync(user);
            return true;
        }


        public async Task UpdateAdmin(UpdateAdminDto model)
        {
            var user = await _userRepo.GetDefault(x => x.Id.Equals(model.Id)) ?? throw new Exception("User not found");

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
            await _userRepo.UpdateAsync(user);
        }

        public async Task<AdminDto> GetByUserName(string userName)
        {
            var user = await _userRepo.GetFilteredFirstOrDefault(
                            select: x => _mapper.Map<AdminDto>(x),
                            where: x => x.UserName.Equals(userName) && x.Status != Status.Passive);

            return user;
        }

        public async Task<bool> UserInRole(string userName, string role)
        {
            var user = await _userManager.FindByNameAsync(userName);
            bool isInRole = await _userManager.IsInRoleAsync(user, role);

            return isInRole;
        }

        public async Task<IEnumerable<AdminDto>> GetAdmins()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            var userDtos = new List<AdminDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var admin = _mapper.Map<Admin>(user);
                var adminDto = _mapper.Map<AdminDto>(admin);
                adminDto.Role = roles.FirstOrDefault();
                adminDto.Email = user.Email;
                userDtos.Add(adminDto);
            }
            return userDtos;
        }
    }
}

