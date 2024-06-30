using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Models.VMs.UserVMs;
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

namespace Ik_Bitirme.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IAppUserRepo _userRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IAdminRepo _adminRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDirectorRepo _directorRepo;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public UserService(IAppUserRepo userRepo, IEmployeeRepo employeeRepo, IAdminRepo adminRepo, UserManager<AppUser> userManager, IDirectorRepo directorRepo, SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _userRepo = userRepo;
            _employeeRepo = employeeRepo;
            _adminRepo = adminRepo;
            _userManager = userManager;
            _directorRepo = directorRepo;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<SignInResult> Login(LoginDto model)
        {
            var user = await _userRepo.GetDefault(x => x.UserName.Equals(model.UserName));
            if (user == null || user.Status == Status.Passive)
            {
                return SignInResult.Failed;
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                return result;
            }
        }

        public async Task<IdentityResult> Register(RegisterDto model)
        {
            var user = _mapper.Map<AppUser>(model);
            //user.ImagePath = $"/images/01-admin.jpg";

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "EMPLOYEE");

                await _signInManager.SignInAsync(user, false);
            }
            return result;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task UpdateUser(UpdateProfileDto model)
        {
            var user = await _userRepo.GetDefault(x => x.Id.Equals(model.Id));

            if (model.ImageData != null)
            {
                user.ImageData = model.ImageData;
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            await _userRepo.UpdateAsync(user);

            if (!string.IsNullOrEmpty(model.Password))
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
        }

        public async Task<UserDto> GetByUserName(string userName)
        {
            var user = await _userRepo.GetFilteredFirstOrDefault(
                            select: x => _mapper.Map<UserDto>(x),
                            where: x => x.UserName.Equals(userName) && x.Status != Status.Passive);

            return user;
        }

        public async Task<bool> UserInRole(string userName, string role)
        {
            var user = await _userManager.FindByNameAsync(userName);
            bool isInRole = await _userManager.IsInRoleAsync(user, role);

            return isInRole;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = await _userRepo.GetDefaults(x => true);
            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Role = roles.FirstOrDefault();
                userDto.Email = user.Email;
                userDtos.Add(userDto);
            }
            return userDtos;
        }

        public async Task<bool> UpdateUserStatus(string userName, string status)
        {
            var user = await _userRepo.GetDefault(x => x.UserName.Equals(userName));

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

            await _userRepo.UpdateAsync(user);
            return true;
        }

        public async Task<UserVM> GetbyUserNameForHome(string userName)
        {
            var user = await _userRepo.GetFilteredFirstOrDefault(
                           select: x => _mapper.Map<UserVM>(x),
                           where: x => x.UserName.Equals(userName) && x.Status != Status.Passive);

            return user;
        }
    }
}
