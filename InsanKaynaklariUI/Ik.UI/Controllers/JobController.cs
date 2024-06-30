using IkMvc.Application.Model.Dto.JobDtos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ik.UI.Controllers
{
    public class JobController : Controller
    {
        private readonly HttpClient _httpClient;
        public JobController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/api/");
          //  _httpClient.BaseAddress = new Uri("https://localhost:7063/api/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Job/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jobs = JsonSerializer.Deserialize<List<JobDto>>(jsonString);
                return View(jobs);
            }
            else
            {
                return View("Error");
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateJobDto model)
        {
            var jsonPayload = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Job/CreateJob/", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Job/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var job = JsonSerializer.Deserialize<UpdateJobDto>(jsonString); 
                return View(job);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateJobDto model)
        {
            var jsonPayload = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"Job/UpdateJob", content); 
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"Job/DeleteJob/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"Job/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var job = JsonSerializer.Deserialize<JobDto>(jsonString);
                return View(job);
            }
            else
            {
                return View("Error");
            }
        }
        

    }
}
