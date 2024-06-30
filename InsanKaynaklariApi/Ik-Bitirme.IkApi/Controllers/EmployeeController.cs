using Ik_Bitirme.Application.Models.DTos.AdminDtos;
using Ik_Bitirme.Application.Models.DTos.EmployeeDtos;
using Ik_Bitirme.Application.Models.VMs.UserVMs;
using Ik_Bitirme.Application.Services.AdminService;
using Ik_Bitirme.Application.Services.EmailServices;
using Ik_Bitirme.Application.Services.EmployeeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;

        public EmployeeController(IEmployeeService employeeService, IEmailService emailService)
        {
            _employeeService = employeeService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployeeDto model)
        {
            var result = await _employeeService.Register(model);
            if (result.Succeeded)
            {
                var welcomeEmailModel = new WelcomeEmailModel
                {
                    ToEmail = model.Email,
                    Username = model.UserName,
                    Email = model.Email,
                    Password = model.Password
                };
                await _emailService.SendWelcomeEmailAsync(welcomeEmailModel);
                return Ok(new { message = "Register successful" });
            }
            return Unauthorized(new { message = "Invalid username or password" });
        }

        [HttpPut("updateemployeestatus/{username}/{status}")]
        public async Task<IActionResult> UpdateEmployeeStatus(string username, string status) 
        {
            var result = await _employeeService.UpdateEmployeeStatus(username, status);
            if (result)
            {
                return Ok(new { message = "Employee status updated successfully" });
            }
            return NotFound(new { message = "Employee not found" });
        }

        [HttpPut("updateemployee")]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeDto model)
        {
            await _employeeService.UpdateEmployee(model);
            return Ok(new { message = "Employee profile updated successfully" });
        }

        [HttpGet("getall")]
        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            var employees = await _employeeService.GetEmployees();
            return employees.ToList();
        }

        [HttpGet("getbyusername/{username}")]
        public async Task<IActionResult> GetEmployeeByUsername(string username)
        {
            var employee = await _employeeService.GetByUserName(username);
            if (employee != null)
            {
                return Ok(employee);
            }
            return NotFound(new { message = "Employee not found" });
        }

        [HttpGet("getemployeesbycompanyid/{companyId}")]
        public async Task<IActionResult> GetEmployeesByCompanyId(int companyId)
        {
            var employees = await _employeeService.GetAllByCompany(companyId);
            if (employees != null)
            {
                return Ok(employees);
            }
            return NotFound(new { message = "Employees not found for the specified company ID" });
        }

        //[HttpPost("createemployee")]
        //public async Task<IActionResult> CreateEmployee(CreateEmployeeDto model)
        //{
        //    var result = await _employeeService.CreateEmployee(model);
        //    if (result)
        //    {
        //        return Ok(new { message = "Employee created successfully" });
        //    }
        //    return BadRequest(new { message = "Failed to create employee" });
        //}

        [HttpDelete("deleteemployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await _employeeService.DeleteEmployee(id);
            if (result)
            {
                return Ok(new { message = "Employee deleted successfully" });
            }
            return NotFound(new { message = "Employee not found" });
        }


    }
}
