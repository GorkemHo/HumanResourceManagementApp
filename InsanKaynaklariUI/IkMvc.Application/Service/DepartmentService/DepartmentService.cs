using IkMvc.Application.Model.Dto.DepartmentDtos;
using IkMvc.Application.Model.Dto.JobDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IkMvc.Application.Service.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private static DepartmentService instance;
        public static DepartmentService Instance { get => instance == null ? new DepartmentService() : instance; }
        private DepartmentService() { }
        public async Task<List<DepartmentDto>> GetAllDepartments(HttpContext httpContext)
        {
            bool tokenStatus = httpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                // Token varsa, HTTP isteği için Authorization header'ına ekleyin
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync("api/Department/getall");

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<DepartmentDto> departments = JsonSerializer.Deserialize<List<DepartmentDto>>(data);
                    return departments;
                }
                return null;
            }
            return null;
        }
    }
}
