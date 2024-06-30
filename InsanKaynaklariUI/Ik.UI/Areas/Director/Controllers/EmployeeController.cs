
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using IkMvc.Application.Model.Dto.CompanyDtos;
using IkMvc.Application.Model.Dto.DirectorDtos;
using IkMvc.Application.Model.Dto.EmployeeDto;
using IkMvc.Application.Model.Dto.EmployeeDtos;
using IkMvc.Application.Model.Enums;
using IkMvc.Application.Model.Vm.PagenationVm;
using IkMvc.Application.Service.CompanyService;
using IkMvc.Application.Service.DepartmentService;
using IkMvc.Application.Service.JobService;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ik.UI.Areas.Director.Controllers
{
    [Area("Director")]
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly IDataProtector _dataProtector;

        public EmployeeController(IDataProtectionProvider dataProtectionProvider)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/api/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _dataProtector = dataProtectionProvider.CreateProtector("EmployeeController");

        }
        public async Task<IActionResult> Index(int page = 1)
        {
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(httpContext: HttpContext);
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var response = await _httpClient.GetAsync("Employee/getall");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var employees = JsonSerializer.Deserialize<List<EmployeeDto>>(jsonString);
                    employees = employees.Where(user => user.Role == "Employee").ToList();

                    employees = employees.Where(user => user.Status == Status.Active).ToList();

                    

                    var username = UserService.Instance.TranslateJWT(jwt);

                    var director = await _httpClient.GetFromJsonAsync<DirectorDto>($"Director/getbyusername/{username}");
                    
                        var directorCompany = director.CompanyId;
                        employees = employees.Where(user => user.CompanyId == directorCompany).ToList();
                    



                    if (employees is not null)
                    {
                        employees.ForEach(employee =>
                        {
                            employee.DecryptedUserName = _dataProtector.Protect(employee.UserName);
                            employee.DecryptedId = _dataProtector.Protect(employee.Id);
                        });
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;
                        int totalItems = employees.Count();
                        var pager = new Pager(totalItems, page, pageSize);
                        int recSkip = (page - 1) * pageSize;
                        var dataContent = employees.Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        return View(dataContent);
                        //return View(employees);
                    }
                    return View();
                }
                else
                {
                    return View("Error");
                }

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeRegister()
        {

            EmployeeRegisterDto dto = new EmployeeRegisterDto();
            
            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var username = UserService.Instance.TranslateJWT(jwt);
                var director = await _httpClient.GetFromJsonAsync<DirectorDto>($"Director/getbyusername/{username}");
                dto.CompanyId = director.CompanyId;
            }
            else
            {
                return Unauthorized();
            }
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(httpContext: HttpContext);
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeRegister(EmployeeRegisterDto model, IFormFile imageFile)
        {
           
            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                if (imageFile != null && imageFile.Length > 0)
                {

                    model.ImageData = ConvertFileToByteArray(imageFile);
                }


                var json = JsonSerializer.Serialize(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Employee/register", data);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Register Success";
                    return RedirectToAction("Index", "Director", new { area = "Director" });
                }
                else
                {
                    TempData["Warning"] = "Register unsuccess";
                    return View(model);
                }
            }
            return Unauthorized();

        }
        [HttpGet]
        public async Task<IActionResult> EmployeeUpdate(string decryptedUserName)
        {
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(httpContext: HttpContext);
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);
            string username = _dataProtector.Unprotect(decryptedUserName);

            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var response = await _httpClient.GetAsync($"Employee/getbyusername/{username}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var employee = JsonSerializer.Deserialize<EmployeeDto>(jsonString);
                    return View(employee);
                }
                else
                {
                    return View("Error");
                }
            }
            return Unauthorized();

        }
        [HttpPost]
        public async Task<IActionResult> EmployeeUpdate(EmployeeDto model, IFormFile imageFile)
        {
            TempData["Jobs"] = JsonSerializer.Serialize(await JobService.Instance.GetAllJobs());
            TempData["Departments"] = JsonSerializer.Serialize(await DepartmentService.Instance.GetAllDepartments(httpContext: HttpContext));
            TempData["Companies"] = JsonSerializer.Serialize(await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext));
            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);
            if (token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                if (imageFile != null && imageFile.Length > 0)
                {
                    model.ImageData = ConvertFileToByteArray(imageFile);
                }
                var json = JsonSerializer.Serialize(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("Employee/updateemployee", data);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Warning"] = "Update unsuccess";
                    return View(model);
                }
            }
            return Unauthorized();

        }
        [HttpGet]
        public async Task<IActionResult> EmployeeDelete(string decryptedId)
        {
            string id = _dataProtector.Unprotect(decryptedId);

            bool token = HttpContext.Request.Cookies.TryGetValue("jwt", out string jwt);

            if (token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                var response = await _httpClient.DeleteAsync($"Employee/deleteemployee/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
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
