using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> RegisterAdmin(AdminRegisterDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _userService.RegisterAdmin(model);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View(model);
        //}
    }
}
