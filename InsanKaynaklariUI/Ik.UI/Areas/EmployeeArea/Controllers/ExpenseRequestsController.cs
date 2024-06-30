using Ik_Bitirme.Application.Models.DTos.UserDtos;
using IkMvc.Application.Model.Dto.ExpenseRequestDtos;
using IkMvc.Application.Model.Enums;
using IkMvc.Application.Model.Vm.ExpenseRequestVm;
using IkMvc.Application.Model.Vm.PagenationVm;
using IkMvc.Application.Model.Vm.UserVms;
using IkMvc.Application.Service.UserService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;


namespace Ik.UI.Areas.EmployeeArea.Controllers
{
    [Area("EmployeeArea")]
    public class ExpenseRequestsController : Controller
    {
        private readonly HttpClient _client;
        public IDataProtector _dataProtector;
        public ExpenseRequestsController(IDataProtectionProvider dataProtector)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/");
            //_client.BaseAddress = new Uri("https://localhost:7063/");
            _dataProtector = dataProtector.CreateProtector("Koruyucu");
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                // Token varsa, HTTP isteği için Authorization header'ına ekleyin
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var response = await _client.GetAsync("api/ExpenseRequests/GetAllExpenseRequests");


                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseRequestVm>>();
                    if (content != null)
                    {
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;
                        int totalItems = content.Count();
                        var pager = new Pager(totalItems, page, pageSize);
                        int recSkip = (page - 1) * pageSize;
                        var datacontent = content.Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        return View(datacontent);
                    }
                    return View();
                }
                else
                {
                    return View();
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByPersonel(int page = 1)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);
            List<ExpenseRequestVm> Vms = new List<ExpenseRequestVm>();

            if (tokenStatus)
            {
                // Token varsa, HTTP isteği için Authorization header'ına ekleyin
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userName = UserService.Instance.TranslateJWT(token);



                var response = await _client.GetAsync("api/ExpenseRequests/GetAllExpenseRequests/");




                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<ExpenseRequestVm>>();
                    if (content != null)
                    {

                        var responseUser = await _client.GetAsync($"api/User/getbyusername/{userName}");

                        if (responseUser.IsSuccessStatusCode)
                        {
                            //var data = await responseUser.Content.ReadAsStringAsync();
                            //var user = JsonSerializer.Deserialize<UserDto>(data);
                            var user = await responseUser.Content.ReadFromJsonAsync<UserDto>();
                            foreach (var expense in content)
                            {
                                if (expense.EmployeeId == user.Id)
                                {
                                    expense.EncryptedId = _dataProtector.Protect(expense.Id.ToString());

                                    Vms.Add(expense);
                                }
                            }

                        }
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;
                        int totalItems = Vms.Count();
                        var pager = new Pager(totalItems, page, pageSize);
                        int recSkip = (page - 1) * pageSize;
                        var datacontent = Vms.Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        return View(datacontent);
                    }
                    return View();
                }
                else
                {
                    return View();
                }
            }
            return Unauthorized();
        }


        [HttpGet]
        public async Task<IActionResult> GetExpenseRequestById(string id)
        {
            string decryptedId = _dataProtector.Unprotect(id);
            int realId = int.Parse(decryptedId);

            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync("api/ExpenseRequests/GetExpenseRequest/" + realId);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<ExpenseRequestVm>();
                    if (content != null)
                    {
                        return View(content);
                    }
                    return View();
                }
                else
                {
                    return View();
                }
            }
            return Unauthorized();
        }


        [HttpGet]
        public ActionResult Create()
        {
            CreateExpenseRequestDto expenseRequest = new CreateExpenseRequestDto();
            TempData["ExpenseTypes"] = Enum.GetValues(typeof(ExpenseType)).Cast<ExpenseType>().ToList();
            TempData["CurrencyType"] = Enum.GetValues(typeof(CurrencyType)).Cast<CurrencyType>().ToList();
            return View(expenseRequest);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateExpenseRequestDto expenseRequest)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);
            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userName = UserService.Instance.TranslateJWT(token);
                var responseUser = await _client.GetAsync($"api/User/getbyusername/{userName}");

                if (responseUser.IsSuccessStatusCode)
                {
                    var user = await responseUser.Content.ReadFromJsonAsync<UserDto>();
                    expenseRequest.EmployeeId = user.Id;
                }
                else
                    TempData["Error"] = "User information could not be retrieved";

                if (expenseRequest.UploadPath != null)
                    expenseRequest.ImageData = ConvertFileToByteArray(expenseRequest.UploadPath);

                var response = await _client.PostAsJsonAsync("api/ExpenseRequests/CreateExpenseRequest/", expenseRequest);


                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Registration Successful";
                    return RedirectToAction("GetAllByPersonel", "ExpenseRequests", new { area = "EmployeeArea" });
                }
                else
                {
                    TempData["Warning"] = "Expenditure request could not be created";
                    TempData["ExpenseTypes"] = Enum.GetValues(typeof(ExpenseType)).Cast<ExpenseType>().ToList();
                    TempData["CurrencyType"] = Enum.GetValues(typeof(CurrencyType)).Cast<CurrencyType>().ToList();
                    return View();
                }

            }
            return Unauthorized();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            string decryptedId = _dataProtector.Unprotect(id);
            int realId = int.Parse(decryptedId);

            TempData["ExpenseTypes"] = Enum.GetValues(typeof(ExpenseType)).Cast<ExpenseType>().ToList();
            TempData["CurrencyType"] = Enum.GetValues(typeof(CurrencyType)).Cast<CurrencyType>().ToList();

            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync("api/ExpenseRequests/UpdateExpenseRequest/" + realId);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<UpdateExpenseRequestDto>();
                    if (content != null)
                        return View(content);

                    return RedirectToAction("GetAllByPersonel", "ExpenseRequests", new { area = "EmployeeArea" });
                }
                return RedirectToAction("GetAllByPersonel", "ExpenseRequests", new { area = "EmployeeArea" });
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UpdateExpenseRequestDto expenseRequest)
        {

            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                // Token varsa, HTTP isteği için Authorization header'ına ekleyin
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var response = await _client.PutAsJsonAsync("api/ExpenseRequests/UpdateExpenseRequest/" + expenseRequest.Id, expenseRequest);


                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Succesfully Updated";
                    return RedirectToAction("GetAllByPersonel", "ExpenseRequests", new { area = "EmployeeArea" });
                }
                return RedirectToAction("GetAllByPersonel", "ExpenseRequests", new { area = "EmployeeArea" });

            }
            return Unauthorized();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            string decryptedId = _dataProtector.Unprotect(id);
            int realId = int.Parse(decryptedId);

            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                // Token varsa, HTTP isteği için Authorization header'ına ekleyin
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var response = await _client.DeleteAsync("api/ExpenseRequests/deleteexpenserequest/" + realId);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Successfully Deleted";
                    return RedirectToAction("getall");
                }
                return RedirectToAction("getall");
            }
            return Unauthorized();
        }

        public byte[] ConvertFileToByteArray(IFormFile file)
        {
            if(file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return Array.Empty<byte>();
        }
    }
}
