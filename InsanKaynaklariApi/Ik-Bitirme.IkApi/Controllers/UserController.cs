using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Services.EmailServices;
using Ik_Bitirme.Application.Services.UserService;
using Ik_Bitirme.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, UserManager<AppUser> userManager, IConfiguration config, IEmailService emailService)
        {
            _userService = userService;
            _userManager = userManager;
            _configuration = config;
            _emailService = emailService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(model);
            if (result.Succeeded)
            {
                return Ok(new { message = "User registered successfully" });
            }
            return BadRequest(new { message = "User registration failed" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _userService.Login(model);
            if (result.Succeeded)
            {
                var userModel=await _userService.GetByUserName(model.UserName);
                var user = await _userManager.FindByIdAsync(userModel.Id);

                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Email,userModel.Email),
                    new Claim(ClaimTypes.Name,userModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                var roles =await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    authClaims.Add(
                    new Claim(ClaimTypes.Role, role)
                    );
                }
               
                var token = GetToken(authClaims);
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(jwt) ;
            }
            return Unauthorized(new { message = "Invalid username or password" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var a = _configuration["JwtSettings:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            var signIn=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                _configuration["JwtSettings:validIssuer"],
                _configuration["JwtSettings:validAudience"],
                authClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signIn
                ) ;

            return token;
        }

        [HttpGet("getbyusername/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {            
            var user = await _userService.GetByUserName(username);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound(new { message = "User not found" });
        }

        [HttpGet("getCurrent")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                UpdateProfileDto model=  new UpdateProfileDto()
                {
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    SecondLastName = user.SecondLastName,
                    Email = user.Email,
                    Address = user.Address,
                    BirthDate = user.BirthDate,
                    BirthPlace = user.BirthPlace,
                    HireDate = user.HireDate,
                    Id = user.Id,
                    JobId = user.JobId,
                    ImageData = user.ImageData,
                    PhoneNumber = user.PhoneNumber,
                    Salary = user.Salary,
                    Status = user.Status,
                    TcIdentity = user.TcIdentity,
                    TerminationDate = user.TerminationDate,
                    UpdateDate= user.UpdateDate,
                    UserName = user.UserName,
                };
                return Ok(model);
            }
            return NotFound(new { message = "User not found" });
        }



        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("getuserhome/{username}")]
        public async Task<IActionResult> GetAllHome(string username)
        {
            var users = await _userService.GetbyUserNameForHome(username);
            return Ok(users);
        }


        [HttpPut("updateuserstatus/{username}/{status}")]
        public async Task<IActionResult> UpdateUserStatus(string username, string status)
        {
            var result = await _userService.UpdateUserStatus(username, status);
            if (result)
            {
                return Ok(new { message = "User status updated successfully" });
            }
            return NotFound(new { message = "User not found" });
        }

        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser(UpdateProfileDto model)
        {
            await _userService.UpdateUser(model);
            return Ok(new { message = "User profile updated successfully" });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogOut();
            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Kullanıcı bulunamadı veya e-posta onaylanmamışsa hata göster
                return NotFound(new { message = "User not found or email not confirmed" });
            }

            // Şifre sıfırlama token'ı oluştur
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // Şifre sıfırlama bağlantısı oluştur
            var callbackUrl = $"{_configuration["AppSettings:ClientUrl"]}insankaynaklari.azurewebsites.net/api/resetpassword?userId={user.Id}&token={token}";

            // E-posta gönderme işlemi
            try
            {
                await _emailService.SendPasswordResetEmailAsync(email, callbackUrl);
                return Ok(new { message = "Şifre sıfırlama bağlantısı e-posta ile gönderildi" });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "E-posta gönderme hatası", error = ex.Message });
            }
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokenBytes = WebEncoders.Base64UrlDecode(model.Token);
            var token = Encoding.UTF8.GetString(tokenBytes);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { message = "Password reset successful" });
            }

            return BadRequest(new { message = "Password reset failed", errors = result.Errors });
        }


    }
}
