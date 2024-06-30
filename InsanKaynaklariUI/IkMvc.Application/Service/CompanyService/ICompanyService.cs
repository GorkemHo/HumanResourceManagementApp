using IkMvc.Application.Model.Dto.CompanyDtos;
using IkMvc.Application.Model.Dto.DepartmentDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkMvc.Application.Service.CompanyService
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetAllCompanies(HttpContext httpContext);
    }
}
