using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ik_Bitirme.Application.Models.DTos.CompanyDto;
using Ik_Bitirme.Application.Models.VMs.CompanyVms;
using Ik_Bitirme.Application.Services.CompanyServices;
using Microsoft.AspNetCore.Authorization;
using Ik_Bitirme.Application.Models.DTos.AdvanceDtos;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<CompanyVm>>> GetAllCompanies()
        {
            var companies = await _companyService.GetAll();
            return Ok(companies);
        }

        [HttpGet("GetbyId")]
        public async Task<ActionResult<CompanyVm>> GetCompanyById(int id)
        {
            var company = await _companyService.GetbyId(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCompany(CreateCompanyDto createCompanyDto)
        {
            await _companyService.Create(createCompanyDto);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCompany(UpdateCompanyDto updateCompanyDto)
        {
            try
            {
                await _companyService.Update(updateCompanyDto);
                return Ok(new { message = "Company updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            await _companyService.Delete(id);
            return Ok();
        }
    }
}
