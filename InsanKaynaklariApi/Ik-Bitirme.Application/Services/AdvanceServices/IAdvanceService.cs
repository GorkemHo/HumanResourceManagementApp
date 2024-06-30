using Ik_Bitirme.Application.Models.DTos.AdvanceDtos;
using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.AdvanceServices
{
    public interface IAdvanceService
    {
        Task Create(CreateAdvanceDto advanceDto);
        Task Update(UpdateAdvanceDtos advanceDto);
        Task Delete(int id);
        Task<AdvanceDto> GetById(int id);
        Task<List<AdvanceDto>> GetAll();        
        Task<List<AdvanceDto>> GetByEmployeeId(string employeeId);
        Task<List<AdvanceDto>> GetByApprovalStatus(ApprovalStatus approvalStatus);
        
    }
}
