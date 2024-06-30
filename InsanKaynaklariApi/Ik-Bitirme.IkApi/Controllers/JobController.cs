using Ik_Bitirme.Application.Models.DTos.JobDtos;
using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Ik_Bitirme.Application.Services.JobService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("CreateJob")]
        public async Task<IActionResult> Create(JobDto model)
        {
            await _jobService.Create(model);
            return Ok(model);
        }
        
        [HttpDelete("DeleteJob/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            await _jobService.Delete(id);
            return Ok(new { message = "Job deleted successfully" });
        }
        

        [HttpPut("UpdateJob")]
        public async Task<IActionResult> UpdateJob(UpdateJobDto model)
        {
            await _jobService.Update(model);
            return Ok(model);
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<List<JobDto>> GetAllJobs()
        {
            var jobs = await _jobService.GetAll();
            return jobs;
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var job = await _jobService.GetById(id);
            if (job != null)
            {
                return Ok(job);
            }
            return NotFound(new { message = "Job not found" });
        }
    }
}
