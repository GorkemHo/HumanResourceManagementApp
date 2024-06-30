using Ik_Bitirme.Application.Models.DTos.EmployeeDtos;
using Ik_Bitirme.Application.Models.DTos.LeaveRequestDtos;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Services.EmployeeServices;
using Ik_Bitirme.Application.Services.LeaveRequestService;
using Ik_Bitirme.Application.Services.UserService;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveRequestController : Controller
    {
        public readonly ILeaveRequestService _leaveRequestService;
        public readonly IEmployeeService _employeeService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService, IEmployeeService employeeService)
        {
            _leaveRequestService = leaveRequestService;
            _employeeService = employeeService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLeaveRequest(CreateLeaveRequestDto createLeaveRequestDto)
        {
            try
            {
                await _leaveRequestService.Create(createLeaveRequestDto);
                return Ok("Leave request created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            try
            {
                await _leaveRequestService.Delete(id);
                return Ok("Leave request deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllLeaveRequests()
        {
            try
            {
                var leaveRequests = await _leaveRequestService.Get();
                return Ok(leaveRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getbycompany/{companyId}")]
        public async Task<IActionResult> GetLeaveRequestsByCompany(int companyId)
        {
            try
            {
                var leaveRequests = await _leaveRequestService.GetbyCompany(companyId);
                return Ok(leaveRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getbycompanywithapprovalstatus/{companyId}/{approvalStatus}")]
        public async Task<IActionResult> GetLeaveRequestsWithApprovalStatus(int companyId, string approvalStatus)
        {
            try
            {
                if (!Enum.TryParse(approvalStatus, out ApprovalStatus status))
                {
                    return BadRequest("Invalid approval status.");
                }

                var leaveRequests = await _leaveRequestService.GetbyCompanywithApprovalStatus(companyId, status);
                return Ok(leaveRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getbyemployee/{UserName}")]
        public async Task<IActionResult> GetLeaveRequestsByEmployee(string userName)
        {
            try
            {
                EmployeeDto user = await _employeeService.GetByUserName(userName);             
                var leaveRequests = await _leaveRequestService.GetbyEmployee(user.Id);
                return Ok(leaveRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetLeaveRequestById(int id)
        {
            try
            {
                var leaveRequest = await _leaveRequestService.GetbyId(id);
                if (leaveRequest == null)
                {
                    return NotFound("Leave request not found.");
                }

                return Ok(leaveRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateLeaveRequest(int id, UpdateLeaveRequestDto model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest("ID mismatch.");
                }

                await _leaveRequestService.Update(model);
                return Ok("Leave request updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
