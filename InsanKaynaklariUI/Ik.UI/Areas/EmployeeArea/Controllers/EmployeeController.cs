using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using IkMvc.Application.Model.Vm.UserVms;
using IkMvc.Application.Service.UserService;
using IkMvc.Application.Model.Dto.UserDtos;
using IkMvc.Application.Model.Dto.EmployeeDto;
using IkMvc.Application.Service.CompanyService;
using IkMvc.Application.Service.DepartmentService;
using IkMvc.Application.Service.JobService;

namespace Ik.UI.Areas.EmployeeArea.Controllers
{
    [Area("EmployeeArea")]
    public class EmployeeController : Controller
    {
        private readonly HttpClient _client;
        
               
        public EmployeeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        }

        
        //public async Task<IActionResult> Index()
        //{
        //    bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

        //    if (tokenStatus)
        //    {
                
        //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        HttpResponseMessage response = await _client.GetAsync("User/getall");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string data = await response.Content.ReadAsStringAsync();
        //            List<UserDto> allUsers = JsonSerializer.Deserialize<List<UserDto>>(data);

                    
        //            List<UserDto> employees = allUsers.Where(user => user.Role == "Employee").ToList();

        //            return View(employees);
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Kullanıcılar alınamadı: {response.StatusCode}");
        //            return Content($"An error occurred while fetching users: {response.StatusCode}");
        //        }
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> Home()
        {
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(httpContext: HttpContext);
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var userName = UserService.Instance.TranslateJWT(token);
                var response = await _client.GetAsync($"api/Employee/getbyusername/{userName}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDto>();
                    return View(user);
                }
                else
                {
                    Console.WriteLine($"Kullanıcı bilgileri alınamadı: {response.StatusCode}");
                    TempData["Error"] = $"An error occurred while fetching user details: {response.StatusCode}";
                    return RedirectToAction("Index", "Error");
                }
            }
            return Unauthorized();
        }

        public async Task<IActionResult> Details()
        {
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(httpContext: HttpContext);
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var userName = UserService.Instance.TranslateJWT(token);
                var response = await _client.GetAsync($"api/Employee/getbyusername/{userName}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDto>();
                    return View(user);
                }
                else
                {
                    Console.WriteLine($"Kullanıcı bilgileri alınamadı: {response.StatusCode}");
                    TempData["Error"] = $"An error occurred while fetching user details: {response.StatusCode}";
                    return RedirectToAction("Index", "Error");
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(httpContext: HttpContext);
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var username = UserService.Instance.TranslateJWT(token);

                var response = await _client.GetAsync($"api/Employee/getbyusername/{username}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var updateProfileDto = JsonSerializer.Deserialize<UpdateProfileDto>(data);
                    return View(updateProfileDto);
                }
                else
                {
                    Console.WriteLine($"Kullanıcı bilgileri alınamadı: {response.StatusCode}");
                    TempData["Error"] = $"An error occurred while fetching user details: {response.StatusCode}";
                    return RedirectToAction("Index", "Error");
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProfileDto model, IFormFile imageFile)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                
                if (imageFile != null && imageFile.Length > 0)
                {
                    model.ImageData = ConvertFileToByteArray(imageFile);
                }

                var jsonData = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync("api/Employee/updateemployee/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Profile updated successfully.";
                    return RedirectToAction("Home","Employee");
                }
                else
                {
                    TempData["Error"] = "An error occurred while updating user";
                    return View(model);
                }
            }
            return Unauthorized();
        }



        [HttpGet]
        public async Task<IActionResult> GetEmployeesByCompanyId(int companyId)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                // Token varsa, HTTP isteği için Authorization header'ına ekleyin
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _client.GetAsync($"api/Employee/getemployeesbycompanyid/{companyId}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    List<EmployeeDto> employees = JsonSerializer.Deserialize<List<EmployeeDto>>(data);

                    return View(employees);
                }
                else
                {
                    Console.WriteLine($"Çalışanlar alınamadı: {response.StatusCode}");
                    return Content($"An error occurred while fetching employees: {response.StatusCode}");
                }
            }
            else
            {
                return Unauthorized();
            }
        }



        public byte[] ConvertFileToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
    

}
