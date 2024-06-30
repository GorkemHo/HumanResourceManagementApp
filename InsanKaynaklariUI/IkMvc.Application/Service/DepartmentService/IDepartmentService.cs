using IkMvc.Application.Model.Dto.DepartmentDtos;
using IkMvc.Application.Model.Dto.JobDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkMvc.Application.Service.DepartmentService
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllDepartments(HttpContext httpContext);
    }
}
