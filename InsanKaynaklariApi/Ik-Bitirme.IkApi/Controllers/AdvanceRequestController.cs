using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.AdvanceDtos;
using Ik_Bitirme.Application.Models.DTos.EmployeeDtos;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Services.AdvanceServices;
using Ik_Bitirme.Application.Services.EmployeeServices;
using Ik_Bitirme.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdvanceRequestController : ControllerBase
    {
        private readonly IAdvanceService _advanceRequestService;
        public readonly IEmployeeService _employeeService;

        public AdvanceRequestController(IAdvanceService advanceRequestService, IEmployeeService employeeService)
        {
            _advanceRequestService = advanceRequestService;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var advances = await _advanceRequestService.GetAll();
                return Ok(advances);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvanceDto advanceDto)
        {
            try
            {
                await _advanceRequestService.Create(advanceDto);
                return StatusCode(StatusCodes.Status201Created, new { message = "Advance request created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _advanceRequestService.Delete(id);
                return Ok(new { message = "Advance request deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAdvanceDtos advanceDto)
        {
            try
            {
                await _advanceRequestService.Update(advanceDto);
                return Ok(new { message = "Advance request updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet("getbyapprovalstatus/{approvalStatus}")]
        public async Task<IActionResult> GetByApprovalStatus(ApprovalStatus approvalStatus)
        {
            try
            {
                var advances = await _advanceRequestService.GetByApprovalStatus(approvalStatus);
                return Ok(advances);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("getbyemployeename/{employeeName}")]
        public async Task<IActionResult> GetByEmployeeName(string employeeName)
        {
            try
            {
                EmployeeDto user = await _employeeService.GetByUserName(employeeName);
                if (user != null)
                {
                    var advances = await _advanceRequestService.GetByEmployeeId(user.Id);
                    return Ok(advances);
                }
                return NotFound($"Employee with username '{employeeName}' not found");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var advance = await _advanceRequestService.GetById(id);
                if (advance == null)
                {
                    
                }
                return Ok(advance);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
    }
}
