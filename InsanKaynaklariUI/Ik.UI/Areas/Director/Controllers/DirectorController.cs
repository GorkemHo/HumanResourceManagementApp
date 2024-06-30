using IkMvc.Application.Model.Dto.AdminDtos;
using IkMvc.Application.Model.Dto.DirectorDtos;
using IkMvc.Application.Model.Dto.ErrorLogDtos;
using IkMvc.Application.Model.Vm.CompanyVm;
using IkMvc.Application.Model.Vm.DirectorVms;
using IkMvc.Application.Service.CompanyService;
using IkMvc.Application.Service.DepartmentService;
using IkMvc.Application.Service.JobService;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Ik.UI.Areas.Director.Controllers
{
    [Area("Director")]
    public class DirectorController : Controller
    {
        public readonly HttpClient _httpClient;
        private readonly IDataProtector _dataProtector;

        public DirectorController(IDataProtectionProvider dataProtectionProvider)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/"); // API'nin base URL'si
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _dataProtector = dataProtectionProvider.CreateProtector("DirectorController");
        }
        public async Task<IActionResult> Index()
        {
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(HttpContext);
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);



            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var username = UserService.Instance.TranslateJWT(jwt);
                var response = await _httpClient.GetFromJsonAsync<DirectorDto>($"api/Director/getbyusername/{username}");
                if (response != null)
                {
                    response.DecryptedUserName = _dataProtector.Protect(response.UserName);
                    return View(response);

                }


                else
                {
                    Console.WriteLine($"Adminler alınamadı: {response}");
                    return Content($"An error occurred while fetching admins: {response}");
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        public async Task<IActionResult> DirectorProfile()
        {
            HttpContext.Request.Cookies.TryGetValue("jwt", out string? jwt);

            var user = await UserService.Instance.GetCurrentDirector(jwt);
            string decryptedUserName = _dataProtector.Protect(user.UserName);
            user.DecryptedUserName = decryptedUserName;


            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EditDirectorProfile()
        {
            HttpContext.Request.Cookies.TryGetValue("jwt", out string? jwt);

            var userVm = await UserService.Instance.GetCurrentDirector(jwt);

            var vm = await UserService.Instance.DirectorVmToDto(userVm);

            string decryptedUserName = _dataProtector.Protect(vm.UserName);
            vm.DecryptedUserName = decryptedUserName;


            if (vm != null)
                return View(vm);
            else
                return RedirectToAction("Index", "Home");
        }
        public byte[] ConvertFileToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditDirectorProfile(UpdateDirectorDto model, IFormFile imageFile)
        {
            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                if (imageFile != null && imageFile.Length > 0)
                {
                    model.ImageData = ConvertFileToByteArray(imageFile);
                }



                var user = await UserService.Instance.CurrentUser(jwt);
                model.Id = user.Id;

                var jsonData = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("api/Director/updatedirector", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("directorprofile", "Director", new { area = "Director" });
                }
                else
                {
                    //CreateErrorLogDto error = new CreateErrorLogDto
                    //{
                    //    UserId = user.Id,
                    //    UserName = user.UserName,
                    //    StatusCode = response.StatusCode,
                    //    Location = "Director Area-Edit Director Profil",
                    //    ErrorMessage = response.Content.ToString()
                    //};
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

    }
}

