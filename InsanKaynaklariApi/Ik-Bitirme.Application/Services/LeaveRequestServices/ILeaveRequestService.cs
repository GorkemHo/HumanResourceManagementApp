using Ik_Bitirme.Application.Models.DTos.LeaveRequestDtos;
using Ik_Bitirme.Application.Models.VMs.LeaveRequestVms;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.LeaveRequestService
{
    public interface ILeaveRequestService
    {
        Task Create(CreateLeaveRequestDto createLeaveRequestDto);
        Task Delete(int Id);
        Task Update(UpdateLeaveRequestDto model);
        Task<List<LeaveRequestVm>> Get();
        Task<LeaveRequestVm> GetbyId(int Id);
        Task<List<LeaveRequestVm>> GetbyEmployee(string Id);
        Task<List<LeaveRequestVm>> GetbyCompany(int Id);
        Task<List<LeaveRequestVm>> GetbyCompanywithApprovalStatus(int Id , ApprovalStatus approvalStatus);
        

    }
}
