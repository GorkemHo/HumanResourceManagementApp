using Ik_Bitirme.Application.Models.DTos.DirectorDtos;
using Ik_Bitirme.Application.Services.DirectorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DirectorController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterDirector(RegisterDirectorDto model)
        {
            var result = await _directorService.Register(model);
            if (result.Succeeded)
            {
                return Ok(new { message = "Register successful" });
            }
            return Unauthorized(new { message = "Invalid username or password" });
        }

        [HttpPut("updatedirectorstatus/{username}/{status}")]
        public async Task<IActionResult> UpdateDirectorStatus(string username, string status)
        {
            var result = await _directorService.UpdateDirectorStatus(username, status);
            if (result)
            {
                return Ok(new { message = "Director status updated successfully" });
            }
            return NotFound(new { message = "Director not found" });
        }

        [HttpPut("updatedirector")]
        public async Task<IActionResult> UpdateDirector(UpdateDirectorDto model)
        {
            await _directorService.UpdateDirector(model);
            return Ok(new { message = "Director profile updated successfully" });
        }

        [HttpGet("getall")]
        public async Task<List<DirectorDto>> GetAllDirectors()
        {
            var directors = await _directorService.GetDirectors();
            return directors.ToList();
        }

        [HttpGet("getbyusername/{username}")]
        public async Task<IActionResult> GetDirectorByUsername(string username)
        {
            var director = await _directorService.GetByUserName(username);
            if (director != null)
            {
                return Ok(director);
            }
            return NotFound(new { message = "Director not found" });
        }

        [HttpGet("getdirectorlist")]
        public async Task<IActionResult> GetDirectorList()
        {
            var directors = await _directorService.GetDirectors();
            if (directors != null)
            {
                return Ok(directors);
            }
            return NotFound(new { message = "Director list not found" });
        }
    }
}
