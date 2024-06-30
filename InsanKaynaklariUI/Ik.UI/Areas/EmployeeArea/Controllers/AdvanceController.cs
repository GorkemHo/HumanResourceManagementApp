using Ik_Bitirme.Application.Models.DTos.UserDtos;
using IkMvc.Application.Model.Dto.AdvanceDtos;
using IkMvc.Application.Model.Vm.PagenationVm;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Ik.UI.Areas.EmployeeArea.Controllers
{
    [Area("EmployeeArea")]
    public class AdvanceController : Controller
    {
        private readonly HttpClient _httpClient;
        private string _employeeId = null;
        public IDataProtector _dataProtector;
        public AdvanceController(IDataProtectionProvider dataProtector)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/"); // API'nin base URL'si
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _dataProtector = dataProtector.CreateProtector("Koruyucu");
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                // Yetkilendirme belirteci HTTP isteğine eklenir
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userName = UserService.Instance.TranslateJWT(token);
                var responseUser = await _httpClient.GetAsync($"api/User/getbyusername/{userName}");
                if (responseUser.IsSuccessStatusCode)
                {
                    var user = await responseUser.Content.ReadFromJsonAsync<UserDto>();
                    var responseAdvance = await _httpClient.GetAsync($"api/AdvanceRequest/getbyemployeename/{user.UserName}");
                    if (responseAdvance.IsSuccessStatusCode)
                    {
                        var jsonString = await responseAdvance.Content.ReadAsStringAsync();
                        var advances = JsonSerializer.Deserialize<AdvanceDto[]>(jsonString).ToList();
                        advances.ForEach(c => c.EncryptedId = _dataProtector.Protect(c.Id.ToString()));
                        if(advances is not null)
                        {
                            const int pageSize = 10;
                            if (page < 1)
                                page = 1;
                            int totalItems = advances.Count();
                            var pager = new Pager(totalItems, page, pageSize);
                            int recSkip = (page - 1) * pageSize;
                            var dataContent = advances.Skip(recSkip).Take(pager.PageSize).ToList();
                            this.ViewBag.Pager = pager;
                            return View(dataContent);
                        }
                        return View();
                    }
                    else
                    {
                        TempData["Error"] = "Avans sayfasina erisiminiz yok";
                        return RedirectToAction("Home", "Employee", new { area = "EmployeeArea" });
                    }
                }
                else
                {
                    TempData["Warning"] = "Mevcut oturumdaki kullanici bulunamadi. Baska bir hesap ile tekrar deneyiniz";
                    return RedirectToAction("Home", "Employee", new { area = "EmployeeArea" });
                }
            }
            else
            {
                TempData["Warning"] = "Token cookie bos";
                return RedirectToAction("Home", "Employee", new { area = "EmployeeArea" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                // Yetkilendirme belirteci HTTP isteğine eklenir
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userName = UserService.Instance.TranslateJWT(token);
                var responseUser = await _httpClient.GetAsync($"api/Employee/getbyusername/{userName}");
                if (responseUser.IsSuccessStatusCode)
                {
                    return View();
                }
                else
                {
                    TempData["Error"] = "Avans sayfasina erisiminiz yok";
                    return RedirectToAction("Home", "Employee", new { area = "EmployeeArea" });
                }
            }
            return RedirectToAction("Home", "Employee", new { area = "EmployeeArea" });


        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvanceDto model)
        {
            if (string.IsNullOrEmpty(_employeeId))
            {
                _employeeId = await GetEmployeeIdAsync();
            }

            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                // Yetkilendirme belirteci HTTP isteğine eklenir
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                model.EmployeeId = _employeeId;
            }

            var jsonPayload = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync("api/AdvanceRequest", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Avans talebi basarili bir sekilde olusturuldu";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Warning"] = "Avans talebi olusturulamadi";
                return RedirectToAction("Home", "Employee", new { area = "EmployeeArea" });
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
            var response = await _httpClient.DeleteAsync($"api/AdvanceRequest/{realId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Advance", new { area = "EmployeeArea" });
            }
            else
            {
                return View("Error");
            }
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
            var response = await _httpClient.GetAsync($"api/AdvanceRequest/{realId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var advance = JsonSerializer.Deserialize<UpdateAdvanceDtos>(jsonString);

                return View(advance);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAdvanceDtos model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                // Yetkilendirme belirteci HTTP isteğine eklenir
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            }
            if (string.IsNullOrEmpty(_employeeId))
            {
                _employeeId = await GetEmployeeIdAsync();
            }
            model.EmployeeId = _employeeId;

            var jsonPayload = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/AdvanceRequest", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }

        private async Task<string> GetEmployeeIdAsync()
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);
            if (tokenStatus)
            {
                // Yetkilendirme belirteci HTTP isteğine eklenir
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userName = UserService.Instance.TranslateJWT(token);
                var responseUser = await _httpClient.GetAsync($"api/Employee/getbyusername/{userName}");
                if (responseUser.IsSuccessStatusCode)
                {
                    var user = await responseUser.Content.ReadFromJsonAsync<UserDto>();
                    return user.Id;
                }
                else
                {
                    return "no";
                }
            }
            else
            {
                return "no";
            }
        }
    }
}
