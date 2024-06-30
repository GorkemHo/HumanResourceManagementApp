using Ik_Bitirme.Application.Models.DTos.UserDtos;
using IkMvc.Application.Model.Dto.UserDtos;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Ik.UI.Controllers
{
    public class UserController : Controller
    {
        HttpClient client;
        public IDataProtector _dataProtector;
        public UserController(IDataProtectionProvider dataProtector)
        {
            client = UserService.Instance.client;
            _dataProtector = dataProtector.CreateProtector("Koruyucu");
        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            string token = await UserService.Instance.Login(model);

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                HttpContext.Response.Cookies.Append("jwt", token);
                var role = await UserService.Instance.GetRoles(token);

                if (role != null)
                {
                    TempData["Success"] = "Login process successful.";
                    if (role == "Admin")
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    else if (role == "Employee")
                        return RedirectToAction("Home", "Employee", new { area = "EmployeeArea" });
                    else if (role == "Director") //director eklendiğinde eklenmesi lazım
                        return RedirectToAction("Index", "Director", new { area = "Director" });
                }
                return RedirectToAction("Index", "Home");

            }
            else
            {
                TempData["Error"] = "Username or Password Not Correct.";
                return RedirectToAction("Login", "User");
            }
        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterDto model)
        {
            var token = UserService.Instance.Register(model);
            if (token != null)
            {
                //return RedirectToAction("Index", "Home");
                TempData["Success"] = "Register process successful.";
                return RedirectToAction("Login", "User");
            }
            else
            {
                return View();
            }
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");
            UserService.Instance.Logout();
            TempData["Success"] = "Logout successful.";
            return RedirectToAction("Home", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            string normalizedEmail = email.ToUpper(new System.Globalization.CultureInfo("en-EN"));
            string url = $"/api/User/forgotpassword?email={normalizedEmail}";

            // İstek gönder
            var response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                ViewBag.Message = message;
                TempData["Success"] = "A password reset link has been sent to your email address. \nPlease check your inbox and follow the instructions to reset your password";
                return RedirectToAction("ForgotPassword", "User");
            }
            else
            {
                TempData["Error"] = "The registered email address could not be found.";
                //return View("Error");
                return RedirectToAction("ForgotPassword", "User");
            }
        }

        public IActionResult ResetPassword(string token, string UserIdToken)
        {
            var decodedUserId = Encoding.UTF8.GetString(Convert.FromBase64String(HttpUtility.UrlDecode(UserIdToken)));

            var model = new ResetPasswordDto
            {
                UserId = decodedUserId,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var response = await client.PostAsJsonAsync("api/user/resetpassword", model);

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                ViewBag.Message = message;
                TempData["Success"] = "Password reset was successful.\nYou can now log in to your account with your new password.";
                return RedirectToAction("Home", "Home");
            }
            else
            {
                TempData["Error"] = "Password reset failed. Please try again.";
                //return View("Error");
                return View(model);
            }
        }

    }
}
