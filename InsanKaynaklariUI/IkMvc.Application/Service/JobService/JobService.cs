using IkMvc.Application.Model.Dto.JobDtos;
using IkMvc.Application.Model.Vm.CompanyVm;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace IkMvc.Application.Service.JobService
{
    public class JobService : IJobService
    {
        private static JobService instance;
        public static JobService Instance { get => instance == null ? new JobService() : instance; }
        private JobService() { }

        public async Task<List<JobDto>> GetAllJobs()
        {
            HttpResponseMessage response =await UserService.UserService.Instance.client.GetAsync("/api/Job/GetAll");

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<JobDto> jobs = JsonSerializer.Deserialize<List<JobDto>>(data);
                return jobs;
            }
            return null;
        }

        public async Task<JobDto> GetById(int? id,string token)
        {

            UserService.UserService.Instance.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage companyResponse = await UserService.UserService.Instance.client.GetAsync("api/Company/GetbyId?id=" + id);
            if (companyResponse.IsSuccessStatusCode)
            {
                var company = await companyResponse.Content.ReadFromJsonAsync<JobDto>();
                return company;
            }
            return null;
        }
    }
}
