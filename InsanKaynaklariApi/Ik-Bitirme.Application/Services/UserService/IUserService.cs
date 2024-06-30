using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Models.VMs.UserVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.UserService
{
    public interface IUserService
    {
        Task<SignInResult> Login(LoginDto model);
        Task<IdentityResult> Register(RegisterDto model);        
        Task LogOut();
        Task UpdateUser(UpdateProfileDto model);
        Task<UserDto> GetByUserName(string userName);
        Task<UserVM> GetbyUserNameForHome(string userName);
        Task<bool> UserInRole(string userName, string role);
        Task<IEnumerable<UserDto>> GetUsers();
        Task<bool> UpdateUserStatus(string userName, string status);
    }
}
