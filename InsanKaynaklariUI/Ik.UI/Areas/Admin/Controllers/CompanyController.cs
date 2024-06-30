using IkMvc.Application.Model.Dto.AdvanceDtos;
using IkMvc.Application.Model.Dto.CompanyDtos;
using IkMvc.Application.Model.Enums;
using IkMvc.Application.Model.Vm.CompanyVm;
using IkMvc.Application.Model.Vm.ExpenseRequestVm;
using IkMvc.Application.Model.Vm.LeaveRequestVm;
using IkMvc.Application.Model.Vm.PagenationVm;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ik.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        public readonly HttpClient _httpClient;
        public IDataProtector _dataProtector;

        public CompanyController(IDataProtectionProvider  dataProtector)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/"); // API'nin base URL'si            
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _dataProtector = dataProtector.CreateProtector("Koruyucu");
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page=1)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("api/Company/getAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var companies = JsonSerializer.Deserialize<CompanyVm[]>(jsonString).ToList();
                    if (companies != null)
                    {                        
                        companies.ForEach(c => c.EncryptedId = _dataProtector.Protect(c.CompanyId.ToString()));
                        const int pageSize = 10;
                        if(page<1)
                            page = 1;
                        int totalItems = companies.Count();
                        var pager = new Pager(totalItems,page, pageSize);
                        int recSkip = (page - 1) * pageSize;
                        var dataCompanion = companies.Skip(recSkip).Take(pager.PageSize).ToList(); 
                        this.ViewBag.Pager = pager;
                        
                        //return View(companies);
                        return View(dataCompanion);
                    }
                    return View();
                }
                else
                {
                    ViewBag.Error = "Company Not Found.";
                    return View();
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userName = UserService.Instance.TranslateJWT(token);
                var responseUser = await _httpClient.GetAsync($"api/Admin/getbyusername/{userName}");

                if (responseUser.IsSuccessStatusCode)
                {
                    return View();
                }
                else
                {
                    TempData["Error"] = "You don't have access to the company page";
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyDto model)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    // Yetkilendirme belirteci HTTP isteğine eklenir
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    if (model.UploadPath != null)
                    {
                        model.ImageData = ConvertFileToByteArray(model.UploadPath);
                    }

                    var jsonPayload = JsonSerializer.Serialize(model);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("api/Company/create", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "Company has been created successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Company could not be created";
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {

            string decryptedId = _dataProtector.Unprotect(id);
            int realId = int.Parse(decryptedId);

            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Company/GetById?id={realId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var company = JsonSerializer.Deserialize<CompanyVm>(jsonString);
                    return View(company);
                }
                else
                {
                    TempData["Error"] = "Details of the selected Company are not available";
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            string decryptedId = _dataProtector.Unprotect(id);
            int realId = int.Parse(decryptedId);

            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                // Yetkilendirme belirteci HTTP isteğine eklenir
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            }
            var response = await _httpClient.GetAsync($"api/Company/GetById?id={realId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var company = JsonSerializer.Deserialize<UpdateCompanyDto>(jsonString);

                return View(company);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCompanyDto model)
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                // Yetkilendirme belirteci HTTP isteğine eklenir
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            }
            model.UpdateDate = DateTime.Now;
            if (model.UploadPath != null)
            {
                model.ImageData = ConvertFileToByteArray(model.UploadPath);
            }

            var jsonPayload = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Company/update", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Update failed!!\nTry again";
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
        }



        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            string decryptedId = _dataProtector.Unprotect(id);
            int realId = int.Parse(decryptedId);

            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                // Yetkilendirme belirteci HTTP isteğine eklenir
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.DeleteAsync($"api/Company/delete?id={realId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Advance", new { area = "EmployeeArea" });
            }
            else
            {
                return View("Error");
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