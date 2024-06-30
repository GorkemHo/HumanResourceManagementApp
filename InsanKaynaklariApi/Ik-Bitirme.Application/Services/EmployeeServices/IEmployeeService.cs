using Ik_Bitirme.Application.Models.DTos.EmployeeDtos;
using Ik_Bitirme.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<bool> CreateEmployee(CreateEmployeeDto model);
        Task<List<Employee>> GetAllByCompany(int id);
        Task<bool> DeleteEmployee(string id);
        Task<IdentityResult> Register(RegisterEmployeeDto model);
        Task<bool> UpdateEmployeeStatus(string userName, string status);
        Task UpdateEmployee(UpdateEmployeeDto model);
        Task<EmployeeDto> GetByUserName(string userName);
        Task<IEnumerable<EmployeeDto>> GetEmployees();
    }
}
