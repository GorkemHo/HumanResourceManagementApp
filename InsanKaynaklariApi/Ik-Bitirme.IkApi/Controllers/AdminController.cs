using Ik_Bitirme.Application.Models.DTos.AdminDtos;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Services.AdminService;
using Ik_Bitirme.Application.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService service, IUserService userService)
        {
            _adminService = service;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminDto model)
        {
            var result = await _adminService.Register(model);
            if (result.Succeeded)
            {
                return Ok(new { message = "Register successful" });
            }
            return Unauthorized(new { message = "Invalid username or password" });
        }


        [HttpPut("updateadminstatus/{username}/{status}")]
        public async Task<IActionResult> UpdateAdminStatus(string username, string status)
        {
            var result = await _adminService.UpdateAdminStatus(username, status);
            if (result)
            {
                return Ok(new { message = "Admin status updated successfully" });
            }
            return NotFound(new { message = "User not found" });
        }

        [HttpPut("updateadmin")]
        public async Task<IActionResult> UpdateAdmin(UpdateAdminDto model)
        {
            await _adminService.UpdateAdmin(model);
            return Ok(new { message = "Admin profile updated successfully" });
        }

        [HttpGet("getall")]
        public async Task<List<AdminDto>> GetAllAdmins()
        {
            var admins = await _adminService.GetAdmins();
            return admins.ToList();
        }
        [HttpGet("getbyusername/{username}")]
        public async Task<AdminDto> GetAdminByUserName(string username)
        {
            var admin = await _adminService.GetByUserName(username);
            return admin;
        }
    }
}
