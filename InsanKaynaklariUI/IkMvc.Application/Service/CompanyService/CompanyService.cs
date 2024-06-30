using IkMvc.Application.Model.Dto.CompanyDtos;
using IkMvc.Application.Model.Dto.DepartmentDtos;
using IkMvc.Application.Model.Vm.CompanyVm;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IkMvc.Application.Service.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private static CompanyService instance;
        public static CompanyService Instance { get => instance == null ? new CompanyService() : instance; }
        private CompanyService() { }
        public async Task<List<CompanyDto>> GetAllCompanies(HttpContext httpContext)
        {
            bool tokenStatus = httpContext.Request.Cookies.TryGetValue("jwt", out string token);

            if (tokenStatus)
            {
                // Token varsa, HTTP isteği için Authorization header'ına ekleyin
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://insankaynaklari.azurewebsites.net/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync("api/Company/getAll");

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<CompanyDto> companies = JsonSerializer.Deserialize<List<CompanyDto>>(data);
                    return companies;
                }
                return null;
            }
            return null;
        }

        public async Task<CompanyVm> GetById(int? id,string token)
        {
            UserService.UserService.Instance.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage companyResponse = await UserService.UserService.Instance.client.GetAsync("api/Company/GetbyId?id=" + id);
        

            if (companyResponse.IsSuccessStatusCode)
            {
                var company = await companyResponse.Content.ReadFromJsonAsync<CompanyVm>();
                return company;
            }
            return null;
        }
    }
}
