using Microsoft.AspNetCore.Mvc;
using IkMvc.Application.Model.Dto.DirectorDtos;
using System.Net.Http.Headers;
using IkMvc.Application.Model.Enums;
using IkMvc.Application.Model.Vm.ExpenseRequestVm;
using System.Reflection;
using IkMvc.Application.Service.JobService;
using IkMvc.Application.Service.DepartmentService;
using IkMvc.Application.Service.CompanyService;
using IkMvc.Application.Model.Vm.PagenationVm;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ik.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DirectorController : Controller
    {
        private readonly HttpClient _client;
        public DirectorController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/");
            //_client.BaseAddress = new Uri("https://localhost:7063/");

        }

        public async Task<IActionResult> GetDirectorList(int page = 1)
        {
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);
            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync("api/Director/getdirectorlist");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<DirectorDto>>();
                    if(content != null)
                    {
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;
                        int totalItems = content.Count();
                        var pager = new Pager(totalItems, page, pageSize);
                        int recSkip = (page - 1) * pageSize;
                        var dataContent = content.Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        return View(dataContent);
                    }                    
                }
                else
                {
                    return NotFound();
                }

            }
            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> CreateDirector()
        {
            RegisterDirectorDto dto = new RegisterDirectorDto();
            TempData["Jobs"] = await JobService.Instance.GetAllJobs();
            TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(httpContext:HttpContext);
            TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext:HttpContext);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDirector(RegisterDirectorDto model)
        {
            bool tokenStatus = HttpContext.Request.Cookies.TryGetValue("jwt", out string token);
            if (tokenStatus)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                if (model.UploadPath != null)
                {
                    model.ImageData = ConvertFileToByteArray(model.UploadPath);
                }
                

                var response = await _client.PostAsJsonAsync("api/Director/register", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Kayit Basarili";
                    return RedirectToAction("GetDirectorList", "Director", new { area = "Admin" });
                }
                else
                {
                    TempData["Jobs"] = await JobService.Instance.GetAllJobs();
                    TempData["Departments"] = await DepartmentService.Instance.GetAllDepartments(httpContext: HttpContext);
                    TempData["Companies"] = await CompanyService.Instance.GetAllCompanies(httpContext: HttpContext);
                    TempData["Warning"] = "Director create unsuccess!";
                    return View(model);
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
