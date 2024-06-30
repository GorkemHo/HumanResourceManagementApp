using Ik_Bitirme.Application.Models.DTos.CompanyDto;
using Ik_Bitirme.Application.Models.DTos.LeaveRequestDtos;
using Ik_Bitirme.Application.Models.VMs.CompanyVms;
using Ik_Bitirme.Application.Models.VMs.LeaveRequestVms;
using Ik_Bitirme.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.CompanyServices
{
    public interface ICompanyService
    {
        Task Create(CreateCompanyDto model);
        Task Delete(int Id);
        Task Update(UpdateCompanyDto model);
        Task<List<CompanyVm>> GetAll();
        Task<CompanyVm> GetbyId(int Id);
    }
}
