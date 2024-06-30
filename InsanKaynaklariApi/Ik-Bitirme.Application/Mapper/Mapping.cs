using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.ExpenseRequestDtos;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Models.DTos.LeaveRequestDtos;
using Ik_Bitirme.Application.Models.VMs.LeaveRequestVms;
using Ik_Bitirme.Application.Models.VMs.ExpenseRequestsVms;
using Ik_Bitirme.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ik_Bitirme.Application.Models.DTos.CompanyDto;
using Ik_Bitirme.Application.Models.VMs.CompanyVms;
using Ik_Bitirme.Application.Models.DTos.DepartmentDto;
using Ik_Bitirme.Application.Models.DTos.AdminDtos;
using Ik_Bitirme.Application.Models.DTos.AdvanceDtos;
using Ik_Bitirme.Application.Models.DTos.DirectorDtos;
using Ik_Bitirme.Application.Models.VMs.DirectorVMs;
using Ik_Bitirme.Application.Models.DTos.JobDtos;
using Ik_Bitirme.Application.Models.VMs.JobVMs;
using Ik_Bitirme.Application.Models.VMs.UserVMs;
using Ik_Bitirme.Application.Models.DTos.EmployeeDtos;
using Ik_Bitirme.Application.Models.DTos.ErrorLogDtos;

namespace Ik_Bitirme.Application.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ExpenseRequest, CreateExpenseRequestDto>().ReverseMap();
            CreateMap<ExpenseRequest, UpdateExpenseRequestDto>().ReverseMap();
            CreateMap<ExpenseRequest, ExpenseRequestVm>().ReverseMap();


            CreateMap<LeaveRequest, CreateLeaveRequestDto>().ReverseMap();
            CreateMap<LeaveRequest, UpdateLeaveRequestDto>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestVm>().ReverseMap();


            CreateMap<AppUser, RegisterDto>().ReverseMap();
            CreateMap<AppUser, LoginDto>().ReverseMap();
            CreateMap<AppUser, UpdateProfileDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UserVM>().ReverseMap();

            CreateMap<AdvanceRequest, AdvanceDto>().ReverseMap();
            CreateMap<AdvanceRequest, UpdateAdvanceDtos>().ReverseMap();
            CreateMap<AdvanceRequest, CreateAdvanceDto>().ReverseMap();

            CreateMap<Director, DirectorDto>().ReverseMap();
            CreateMap<Director, UpdateDirectorDto>().ReverseMap();
            CreateMap<Director, RegisterDirectorDto>().ReverseMap();
            CreateMap<Director, DirectorVM>().ReverseMap();

            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<Job, UpdateJobDto>().ReverseMap();
            CreateMap<Job, JobVM>().ReverseMap();


            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<Admin, RegisterAdminDto>().ReverseMap();
            CreateMap<Admin, UpdateAdminDto>().ReverseMap();
            CreateMap<Admin, AppUser>().ReverseMap();

            CreateMap<ExpenseRequest, UpdateExpenseRequestDto>().ReverseMap();
            CreateMap<ExpenseRequest, ExpenseRequestVm>().ReverseMap();

            CreateMap<Department, CreateDepartmentDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Department, UpdateDepartmentDto>().ReverseMap();

            //CreateMap<List<Employee>, List<AppUser>>().ReverseMap();


            CreateMap<Company, CreateCompanyDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyDto>().ReverseMap();
            CreateMap<Company, CompanyVm>().ReverseMap();

            CreateMap<Employee, RegisterEmployeeDto>().ReverseMap();
            CreateMap<Employee, UpdateEmployeeDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, AppUser>().ReverseMap();

            CreateMap<ErrorLog, CreateErrorLogDto>().ReverseMap();

        }

    }
}
