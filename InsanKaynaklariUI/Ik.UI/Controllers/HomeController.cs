using Ik_Bitirme.Application.Models.DTos.UserDtos;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Ik.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var token = HttpContext.Request.Cookies["jwt"];
          
         
            var b = await UserService.Instance.CurrentUser(token);
            return View();
        }

        public async Task<IActionResult> Home()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            return View();
        }

        public IActionResult ErrorPage500()
        {
            return View();
        }

        public IActionResult ErrorPage404()
        {
            return View();
        }
    }
}
