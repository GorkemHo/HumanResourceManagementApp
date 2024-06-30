using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.CompanyDto;
using Ik_Bitirme.Application.Models.VMs.CompanyVms;
using Ik_Bitirme.Application.Models.VMs.LeaveRequestVms;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Domain.IRepositories;
using Ik_Bitirme.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.CompanyServices
{
    public class CompanyService : ICompanyService
    {
        public readonly IMapper _mapper;
        public readonly ICompanyRepo _companyRepo;

        public CompanyService(IMapper mapper, ICompanyRepo companyRepo)
        {
            _mapper = mapper;
            _companyRepo = companyRepo;
        }

        public async Task Create(CreateCompanyDto model)
        {
            var company = _mapper.Map<Company>(model);
            await _companyRepo.CreateAsync(company);
        }

        public async Task Delete(int Id)
        {
            var company = await _companyRepo.GetDefault(l => l.CompanyId == Id);
            if(company is not null)
            {
                company.Status = Status.Passive;
                company.DeleteDate = DateTime.Now;
                await _companyRepo.DeleteAsync(company);
            }

        }

        public async Task<List<CompanyVm>> GetAll()
        {
            var companies = await _companyRepo.GetFilteredList(
                select: x => _mapper.Map<CompanyVm>(x),
                where: x => !x.Status.Equals(Status.Passive),
                orderby: x => x.OrderBy(x => x.Name));
            return companies;
        }

        public async Task<CompanyVm> GetbyId(int Id)
        {
            var company = await _companyRepo.GetDefault(x => x.CompanyId == Id);
            if (company == null)
            {
                return null;
            }
            var CompanyVm = _mapper.Map<CompanyVm>(company);
            return CompanyVm;
        }

        public async Task Update(UpdateCompanyDto model)
        {
            var company = _mapper.Map<Company>(model);
            company.UpdateDate = DateTime.Now;
            await _companyRepo.UpdateAsync(company);
        }
    }
}
