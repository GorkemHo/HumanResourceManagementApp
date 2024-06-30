using Ik_Bitirme.Application.Models.DTos.ErrorLogDtos;
using Ik_Bitirme.Application.Services.ErrorLogServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ik_Bitirme.IkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorLogController : ControllerBase
    {
        public readonly IErrorLogService _service;

        public ErrorLogController(IErrorLogService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateErrorLogDto errorLogDto)
        {
            await _service.Create(errorLogDto);
            return Ok();
        }
    }
}
