using IkMvc.Application.Model.Dto.AdminDtos;
using IkMvc.Application.Service.DepartmentService;
using IkMvc.Application.Service.JobService;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ik.UI.Areas.Admin.Controllers
{
    [Area("Admin")]    
    public class AdminController : Controller
    {
        private readonly HttpClient _client;
        
        public AdminController()
        {

            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/");            

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<IActionResult> Index()
        {
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(HttpContext);
            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {               
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var username= UserService.Instance.TranslateJWT(jwt);
                var response = await _client.GetAsync($"api/Admin/getbyusername/{username}");
                  if (response.IsSuccessStatusCode)
                 {
                    
                    var data = await response.Content.ReadAsStringAsync();
                    var admins = JsonSerializer.Deserialize<AdminDto>(data);                   
                    return View(admins);
                }
                else
                {
                    Console.WriteLine($"Adminler alınamadı: {response.StatusCode}");
                    return Content($"An error occurred while fetching admins: {response.StatusCode}");
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> UpdateAdmin()
        {

            bool tokenstatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (tokenstatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var username = UserService.Instance.TranslateJWT(jwt);
                var response = await _client.GetAsync("api/Admin/getbyusername/" + username);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var admin = JsonSerializer.Deserialize<UpdateAdminDto>(data);
                    return View(admin);
                }
                else
                {
                    Console.WriteLine($"Admin alınamadı: {response.StatusCode}");
                    return Content($"An error occurred while fetching admin: {response.StatusCode}");
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(UpdateAdminDto model ,IFormFile imageFile)
        {            
            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {
                if (imageFile != null && imageFile.Length>0)
                {
                    model.ImageData = ConvertFileToByteArray(imageFile);

                }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync("api/Admin/updateadmin", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //Console.WriteLine($"Admin güncellenemedi: {response.StatusCode}");
                    //return Content($"An error occurred while updating admin: {response.StatusCode}");
                    return View(model);
                }

            }
            else
            {
                return Unauthorized();
            }

        }
        public async Task<IActionResult> Details()
        {
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(HttpContext);
            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string? jwt);
            if (token)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var username = UserService.Instance.TranslateJWT(jwt!);
                var response = await _client.GetAsync("api/Admin/getbyusername/" + username);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var admin = JsonSerializer.Deserialize<AdminDto>(data);
                    return View(admin);
                }
                else
                {
                    Console.WriteLine($"Admin alınamadı: {response.StatusCode}");
                    return Content($"An error occurred while fetching admin: {response.StatusCode}");
                }
            }
            return Unauthorized();
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
