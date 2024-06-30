using Ik_Bitirme.Application.Models.DTos.UserDtos;
using IkMvc.Application.Model.Dto.LeaveRequestDtos;
using IkMvc.Application.Model.Enums;
using IkMvc.Application.Model.Vm.ExpenseRequestVm;
using IkMvc.Application.Model.Vm.LeaveRequestVm;
using IkMvc.Application.Model.Vm.PagenationVm;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Ik.UI.Areas.EmployeeArea.Controllers
{
    [Area("EmployeeArea")]
    public class LeaveRequestController : Controller
    {
        private readonly HttpClient _httpClient;
        public IDataProtector _dataProtector;
        public LeaveRequestController(IDataProtectionProvider dataProtector)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _dataProtector = dataProtector.CreateProtector("Koruyucu");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);


            if (tokenStatus)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userName = UserService.Instance.TranslateJWT(token);
                try
                {
                    var response = await _httpClient.GetAsync($"/api/LeaveRequest/getbyemployee/{userName}");

                    if (response.IsSuccessStatusCode)
                    {
                        var leaveRequests = await response.Content.ReadFromJsonAsync<IEnumerable<LeaveRequestVm>>();
                        if (leaveRequests != null)
                        {
                            leaveRequests.ToList().ForEach(c => c.EncryptedId = _dataProtector.Protect(c.Id.ToString()));
                            const int pageSize = 10;
                            if (page < 1)
                                page = 1;
                            int totalItems = leaveRequests.Count();
                            var pager = new Pager(totalItems, page, pageSize);
                            int recSkip = (page - 1) * pageSize;
                            var dataleaveRequests = leaveRequests.Skip(recSkip).Take(pager.PageSize).ToList();
                            this.ViewBag.Pager = pager;
                            return View(dataleaveRequests);
                        }
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "İzin istekleri alınamadı.";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
                    return View();
                }
            }
            return Unauthorized();
        }

        public async Task<IActionResult> Details(string id)
        {
            string decryptedId = _dataProtector.Unprotect(id);
            int realId = int.Parse(decryptedId);

            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);


            if (tokenStatus)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var response = await _httpClient.GetAsync($"/api/LeaveRequest/getbyid/{realId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var leaveRequests = await response.Content.ReadFromJsonAsync<LeaveRequestVm>();
                        if (leaveRequests != null)
                        {
                            return View(leaveRequests);
                        }
                        return View();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ViewBag.Error = "İzin isteği Bulunamadı.";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Bir hata oluştu.";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateLeaveRequestDto model = new CreateLeaveRequestDto
            {
                EmployeeId = "string",
                NumberOfDays = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                ApprovalStatus = ApprovalStatus.Pending,
                LeaveType = " ",
                RequestDate = DateTime.Now,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveRequestDto model)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);


            if (tokenStatus)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userName = UserService.Instance.TranslateJWT(token);
                var responseUser = await _httpClient.GetAsync($"/api/User/getbyusername/{userName}");

                if (responseUser.IsSuccessStatusCode)
                {
                    var data = await responseUser.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<UserDto>(data);
                    model.EmployeeId = user.Id;
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var response = await _httpClient.PostAsJsonAsync("/api/LeaveRequest/create", model);

                            if (response.IsSuccessStatusCode)
                            {

                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ViewBag.Error = "İzin isteği oluşturulamadı.";
                                return View(model);
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
                            return View(model);
                        }
                    }
                    else
                    {
                        return View(model);
                    }

                }
                else
                {
                    Console.WriteLine($"Kullanıcı bilgileri alınamadı: {responseUser.StatusCode}");

                }
            }
            return View(model);

        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            string decryptedId = _dataProtector.Unprotect(id);
            int realId = int.Parse(decryptedId);

            var token = HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            }
            var response = await _httpClient.DeleteAsync($"api/LeaveRequest/delete/{realId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "LeaveRequest", new { area = "EmployeeArea" });
            }
            else
            {
                return View("Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            string decryptedId = _dataProtector.Unprotect(Id);
            int realId = int.Parse(decryptedId);

            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);


            if (tokenStatus)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var response = await _httpClient.GetAsync($"/api/LeaveRequest/getbyid/{realId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var leaveRequests = await response.Content.ReadFromJsonAsync<LeaveRequestVm>();
                        if (leaveRequests != null)
                        {
                            return View(leaveRequests);
                        }
                        return View();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ViewBag.Error = "İzin isteği Bulunamadı.";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Bir hata oluştu.";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateLeaveRequestDto model)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);


            if (tokenStatus)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                if (ModelState.IsValid)
                {
                    try
                    {
                        var response = await _httpClient.PutAsJsonAsync($"/api/LeaveRequest/update/{model.Id}", model);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Error = "İzin isteği güncellenemedi.";
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
                        return View();
                    }
                }
                else
                {
                    return View(model);
                }
            }
            return RedirectToAction("Index");

        }
    }
}
