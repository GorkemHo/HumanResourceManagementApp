using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using IkMvc.Application.Model.Dto.DirectorDtos;
using System.Net.Http.Json;
using IkMvc.Application.Model.Vm.DirectorVms;
using IkMvc.Application.Model.Vm.CompanyVm;
using IkMvc.Application.Model.Dto.JobDtos;
using IkMvc.Application.Model.Dto.DepartmentDtos;

namespace IkMvc.Application.Service.UserService
{
    public class UserService : IUserService
    {
        public readonly Uri baseAdress = new Uri("https://insankaynaklari.azurewebsites.net/");


        //public Uri baseAdress = new Uri("https://localhost:7063/api/");

        public HttpClient client;
        private static UserService _instance;
        public static UserService Instance { get => _instance == null ? new UserService() : _instance; }


        private UserService()
        {
            client = new HttpClient();
            client.BaseAddress = baseAdress;
        }

        public async Task<DirectorVm> GetCurrentDirector(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var username = TranslateJWT(token);
            HttpResponseMessage response = await client.GetAsync("api/Director/getbyusername/" + username);

            if (response.IsSuccessStatusCode)
            {
                var dir = await response.Content.ReadFromJsonAsync<DirectorDto>();

                var vm = new DirectorVm()
                {
                    FirstName = dir.FirstName,
                    LastName = dir.LastName,
                    Email = dir.Email,
                    UserName = dir.UserName,
                    DepartmentId = dir.DepartmentId,
                    JobId = dir.JobId,
                    Salary = dir.Salary,
                    // Department = dir.Department,
                    Address = dir.Address,
                    CompanyId = dir.CompanyId,
                    CreateDate = dir.CreateDate,
                    BirthPlace = dir.BirthPlace,
                    BirthDate = dir.BirthDate,
                    DeleteDate = dir.DeleteDate,
                    HireDate = dir.HireDate,
                    ImageData = dir.ImageData,
                    MiddleName = dir.MiddleName,
                    PhoneNumber = dir.PhoneNumber,
                    Role = dir.Role,
                    SecondLastName = dir.SecondLastName,
                    Status = dir.Status,
                    TcIdentity = dir.TcIdentity,
                    TerminationDate = dir.TerminationDate,
                    UpdateDate = dir.UpdateDate,
                };

                HttpResponseMessage companyResponse = await client.GetAsync("api/Company/GetbyId?id=" + vm.CompanyId);
                HttpResponseMessage jobResponse = await client.GetAsync("/api/Job/GetById/" + vm.JobId);

                HttpResponseMessage departmentResponse = await client.GetAsync("/api/Department/getbyid/" + vm.DepartmentId);


                if (companyResponse.IsSuccessStatusCode)
                {
                    var company = await companyResponse.Content.ReadFromJsonAsync<CompanyVm>();
                    vm.Company = company;
                }

                if (jobResponse.IsSuccessStatusCode)
                {
                    var job = await jobResponse.Content.ReadFromJsonAsync<JobDto>();
                    vm.Job = job;
                }

                if (departmentResponse.IsSuccessStatusCode)
                {
                    var dep = await departmentResponse.Content.ReadFromJsonAsync<DepartmentDto>();
                    vm.Department = dep;
                }

                return vm;
            }
            return null;
        }

        public async Task<UpdateDirectorDto> DirectorVmToDto(DirectorVm dir)
        {

            var dto = new UpdateDirectorDto()
            {
                FirstName = dir.FirstName,
                LastName = dir.LastName,
                Email = dir.Email,
                UserName = dir.UserName,
                DepartmentId = dir.DepartmentId,
                JobId = dir.JobId,
                Salary = dir.Salary,
                Address = dir.Address,
                CompanyId = dir.CompanyId,

                BirthPlace = dir.BirthPlace,
                BirthDate = dir.BirthDate,

                HireDate = dir.HireDate,
                ImageData = dir.ImageData,
                MiddleName = dir.MiddleName,
                PhoneNumber = dir.PhoneNumber,

                SecondLastName = dir.SecondLastName,
                Status = dir.Status,
                TcIdentity = dir.TcIdentity,
                TerminationDate = dir.TerminationDate,
                UpdateDate = dir.UpdateDate,
            };

            return dto;
        }


        public async Task<UpdateProfileDto> CurrentUser(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var username = TranslateJWT(token);


            HttpResponseMessage response = await client.GetAsync("api/User/getbyusername/" + username);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                UpdateProfileDto user = JsonSerializer.Deserialize<UpdateProfileDto>(data);

                return user;
            }

            return null;

        }


        public string TranslateJWT(string token)
        {
            string username = string.Empty;
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jsonToken != null)
                {
                    username = jsonToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                    return username;
                }
            }
            return null;
        }

        public async Task<string> GetRoles(string token)
        {
            string role = string.Empty;
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jsonToken != null)
                {
                    role = jsonToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
                    return role;
                }
            }
            return null;
        }

        public async Task<string> Login(LoginDto model)
        {
            var jsonData = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/User/login", content);


            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return token;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> Register(RegisterDto model)
        {
            var jsonData = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/User/Register", content);

            if (response.IsSuccessStatusCode)
            {
                LoginDto dto = new LoginDto
                {
                    UserName = model.UserName,
                    Password = model.Password,
                };

                return await Login(dto);
            }
            else
            {
                return null;
            }
        }

        public async Task Logout()
        {
            client.DefaultRequestHeaders.Authorization = null;
            await client.GetAsync("https://insankaynaklari.azurewebsites.net/api/User/logout");
        }



    }
}
