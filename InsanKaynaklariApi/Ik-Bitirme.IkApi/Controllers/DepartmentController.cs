using Ik_Bitirme.Application.Models.DTos.DepartmentDto;
using Ik_Bitirme.Application.Services.DepartmentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            await _departmentService.Create(model);
            return Ok(new { message = "Department created successfully" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.Delete(id);
            return Ok(new { message = "Department deleted successfully" });
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAll();
            return Ok(departments);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetById(id);
            if (department != null)
            {
                return Ok(department);
            }
            return NotFound(new { message = "Department not found" });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateDepartmentDto model)
        {
            await _departmentService.Update(model);
            return Ok(new { message = "Department updated successfully" });
        }
    }
}
