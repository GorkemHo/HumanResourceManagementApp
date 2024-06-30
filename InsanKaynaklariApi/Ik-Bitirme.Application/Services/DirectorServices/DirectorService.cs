using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.DirectorDtos;
using Ik_Bitirme.Application.Models.DTos.EmployeeDtos;
using Ik_Bitirme.Application.Models.VMs.DirectorVMs;
using Ik_Bitirme.Application.Services.DirectorService;
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

namespace Ik_Bitirme.Application.Services.DirectorServices
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepo _directorRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICompanyRepo _companyRepository;
        private readonly IDepartmentRepo _departmentRepository;
        public DirectorService(IDirectorRepo repo, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ICompanyRepo companyRepository, IDepartmentRepo departmentRepository)
        {
            _directorRepo = repo;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IdentityResult> Register(RegisterDirectorDto model)
        {
            var user = _mapper.Map<Director>(model);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "DIRECTOR");
                await _signInManager.SignInAsync(user, false);
            }
            return result;
        }

        public async Task<bool> UpdateDirectorStatus(string userName, string status)
        {
            var user = await _directorRepo.GetDefault(x => x.UserName.Equals(userName));

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

            await _directorRepo.UpdateAsync(user);
            return true;
        }

        public async Task UpdateDirector(UpdateDirectorDto model)
        {
            var user = await _directorRepo.GetDefault(x => x.Id.Equals(model.Id)) ?? throw new Exception("User not found");

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
            await _directorRepo.UpdateAsync(user);
        }

        public async Task<DirectorDto> GetByUserName(string userName)
        {
            var user = await _directorRepo.GetFilteredFirstOrDefault(
                            select: x => _mapper.Map<DirectorDto>(x),
                            where: x => x.UserName.Equals(userName) && x.Status != Status.Passive);

            return user;
        }

        public async Task<IEnumerable<DirectorDto>> GetDirectors()
        {
            var users = await _userManager.GetUsersInRoleAsync("Director");
            var userDtos = new List<DirectorDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var director = _mapper.Map<Director>(user);
                var directorDto = _mapper.Map<DirectorDto>(director);
                directorDto.Role = roles.FirstOrDefault();
                directorDto.Email = user.Email;
                userDtos.Add(directorDto);
            }
            return userDtos;
        }

        public async Task<List<Director>> GetAllByCompany(int id)
        {
            List<Director> result = new List<Director>();
            List<Company> companies = await _companyRepository.GetDefaults(x => x.Status != Status.Passive);
            List<Department> departments = await _departmentRepository.GetDefaults(x => x.Status != Status.Passive);

            var employeeList = await _userManager.GetUsersInRoleAsync("Director");

            var list = _mapper.Map<List<Director>>(employeeList);

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
