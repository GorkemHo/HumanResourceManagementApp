using Ik_Bitirme.Application.Models.DTos.AdminDtos;
using Ik_Bitirme.Application.Models.DTos.CompanyDto;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.AdminService
{
    public interface IAdminService
    {
  
        Task<IdentityResult> Register(RegisterAdminDto model);
        
        Task UpdateAdmin(UpdateAdminDto model);
        Task<bool> UpdateAdminStatus(string userName, string status);
        Task<AdminDto> GetByUserName(string userName);
        Task<IEnumerable<AdminDto>> GetAdmins();
        

     }
}
