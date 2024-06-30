using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Infrastructure.Context;
using Ik_Bitirme.Application.Services.ExpenseRequestServices;
using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.ExpenseRequestDtos;
using Microsoft.AspNetCore.Authorization;
using Ik_Bitirme.Application.Models.VMs.ExpenseRequestsVms;
using Ik_Bitirme.Application.Models.DTos.EmployeeDtos;
using Ik_Bitirme.Application.Services.EmployeeServices;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ExpenseRequestsController : ControllerBase
    {
        private readonly IExpenseRequestService _expenseRequestService;
        private readonly IMapper _mapper;
        public readonly IEmployeeService _employeeService;

        public ExpenseRequestsController(IExpenseRequestService expenseRequestService, IMapper mapper, IEmployeeService employeeService)
        {
            _expenseRequestService = expenseRequestService;
            _mapper = mapper;
            _employeeService = employeeService;
        }



        // GET: api/ExpenseRequests
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<ExpenseRequestVm>>> GetAllExpenseRequests()
        {

            List<ExpenseRequestVm> list = await _expenseRequestService.GetAllExpenseRequests();
            return list;
        }

        // GET: api/ExpenseRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseRequestVm>> GetExpenseRequest(int id)
        {

            var expenseRequest = await _expenseRequestService.GetById(id);
            var expenseRequestVm = _mapper.Map<ExpenseRequestVm>(expenseRequest);

            if (expenseRequest == null)
            {
                return NotFound();
            }

            return expenseRequestVm;
        }

        // POST: api/ExpenseRequests
        [HttpPost]
        public async Task<ActionResult<ExpenseRequest>> CreateExpenseRequest(CreateExpenseRequestDto expenseRequest)
        {

            var result = await _expenseRequestService.CreateExpenseRequest(expenseRequest);

            if (result)
                return Ok(result);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateExpenseRequest(int id)
        {
            ExpenseRequest expenseRequestVm = await _expenseRequestService.GetById(id);
            UpdateExpenseRequestDto updateSpendDto = _mapper.Map<UpdateExpenseRequestDto>(expenseRequestVm);
            if (updateSpendDto != null)
            {
                return Ok(updateSpendDto);
            }
            else
            {
                return NotFound("Harcama Bulunamadı");
            }
        }

        // PUT: api/ExpenseRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpenseRequest(int id, UpdateExpenseRequestDto expenseRequest)
        {
            if (id != expenseRequest.Id)
            {
                return BadRequest();
            }


            var result = await _expenseRequestService.UpdateExpenseRequest(expenseRequest);
            if (result)
                return Ok(result);
            else
                return NoContent();
        }

        // DELETE: api/ExpenseRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseRequest(int id)
        {
            var result = await _expenseRequestService.SoftDeleteExpenseRequest(id);

            if (result)
                return Ok(result);
            else
                return NoContent();


        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetByEmployeeName(string userName)
        {
            try
            {
                EmployeeDto user = await _employeeService.GetByUserName(userName);
                var advances = await _expenseRequestService.GetByEmployeeId(user.Id);
                return Ok(advances);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetApprovedList()
        {

            return Ok(await _expenseRequestService.GetApprovedList());
        }


        [HttpGet]
        //[Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetRejectList()
        {

            return Ok(await _expenseRequestService.GetRejectList());
        }

        [HttpGet()]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveExpense(int id)
        {
            if (await _expenseRequestService.ApproveExpense(id))
            {
                return Ok("İşlem başarılı");
            }

            return BadRequest("İşlem sırasında hata oluştu");
        }

        [HttpGet()]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectExpense(int id)
        {
            if (await _expenseRequestService.RejectExpense(id))
            {
                return Ok("İşlem başarılı");
            }

            return BadRequest("İşlem sırasında hata oluştu");
        }


        [HttpGet]
        //[Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetPendingList()
        {

            return Ok(await _expenseRequestService.GetPendingList());
        }

    }
}
