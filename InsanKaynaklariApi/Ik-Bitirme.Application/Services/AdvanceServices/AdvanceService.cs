using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.AdvanceDtos;
using Ik_Bitirme.Application.Models.VMs;
using Ik_Bitirme.Application.Models.VMs.AdvanceVMs;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.AdvanceServices
{
    public class AdvanceService : IAdvanceService
    {
        private readonly IAdvanceRequestRepo _repo;
        private readonly IMapper _mapper;

        public AdvanceService(IAdvanceRequestRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task Create(CreateAdvanceDto advanceDto)
        {
            var advance = _mapper.Map<AdvanceRequest>(advanceDto);
            await _repo.CreateAsync(advance);
        }

        public async Task Delete(int id)
        {
            AdvanceRequest advance = await _repo.GetDefault(x => x.Id == id);
            await _repo.DeleteAsync(advance);
        }

        public async Task<List<AdvanceDto>> GetAll()
        {
            var advances = await _repo.GetDefaults(x => x.Status != Status.Passive);
            return _mapper.Map<List<AdvanceDto>>(advances);
        }

        public async Task<List<AdvanceDto>> GetByEmployeeId(string employeeId)
        {
            var advances = await _repo.GetFilteredList(
                select: x => _mapper.Map<AdvanceDto>(x),
                where: x => x.EmployeeId == employeeId && x.Status != Status.Passive,
                orderby: x => x.OrderBy(x => x)
            );
            return advances;
        }



        public async Task<AdvanceDto> GetById(int id)
        {
            var advance = await _repo.GetDefault(x => x.Id == id);
            return _mapper.Map<AdvanceDto>(advance);
        }

        public async Task Update(UpdateAdvanceDtos advanceDto)
        {
            var advance = _mapper.Map<AdvanceRequest>(advanceDto);
            await _repo.UpdateAsync(advance);
        }

        public async Task<List<AdvanceDto>> GetByApprovalStatus(ApprovalStatus approvalStatus)
        {
            var advances = await _repo.GetFilteredList(select: x => _mapper.Map<AdvanceDto>(x),
        where: x => x.ApprovalStatus == approvalStatus && x.Status != Status.Passive,
       orderby: x => x.OrderBy(x => x)
   );
            return advances;
        }
    }
}
