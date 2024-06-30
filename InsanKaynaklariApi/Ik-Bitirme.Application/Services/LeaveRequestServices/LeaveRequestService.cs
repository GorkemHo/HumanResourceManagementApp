using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.LeaveRequestDtos;
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

namespace Ik_Bitirme.Application.Services.LeaveRequestService
{
    public class LeaveRequestService : ILeaveRequestService
    {
        public readonly ILeaveRequestRepo _leaveRequestRepo;
        public readonly IMapper _mapper;
        public LeaveRequestService(ILeaveRequestRepo leaveRequestRepo, IMapper mapper)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _mapper = mapper;
        }

        public async Task Create(CreateLeaveRequestDto createLeaveRequestDto)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(createLeaveRequestDto);
            await _leaveRequestRepo.CreateAsync(leaveRequest);
        }

        public async Task Delete(int Id)
        {
            var leaveRequest = await _leaveRequestRepo.GetDefault(l => l.Id == Id);
            if (leaveRequest is not null)
            {
                leaveRequest.DeleteDate = DateTime.Now;
                leaveRequest.Status = Status.Passive;
                await _leaveRequestRepo.DeleteAsync(leaveRequest);
            }
        }

        public async Task<List<LeaveRequestVm>> Get()
        {
            var leaveRequest = await _leaveRequestRepo.GetFilteredList(
                select: x => _mapper.Map<LeaveRequestVm>(x),
                where: x => !x.Status.Equals(Status.Passive),
                orderby: x => x.OrderBy(x => x.Id));
            return leaveRequest;
        }

        public async Task<List<LeaveRequestVm>> GetbyCompany(int Id)
        {
            var leaveRequests = await _leaveRequestRepo.GetFilteredList(
                select: x => _mapper.Map<LeaveRequestVm>(x),
                where: x => !x.Status.Equals(Status.Passive) && x.Employee.CompanyId == Id,
                orderby: x => x.OrderBy(x => x.Id));

            return leaveRequests;
        }

        public async Task<List<LeaveRequestVm>> GetbyCompanywithApprovalStatus(int Id, ApprovalStatus approvalStatus)
        {
            var leaveRequests = await _leaveRequestRepo.GetFilteredList(
               select: x => _mapper.Map<LeaveRequestVm>(x),
               where: x => !x.Status.Equals(Status.Passive) && x.Employee.CompanyId == Id && x.ApprovalStatus == approvalStatus,
               orderby: x => x.OrderBy(x => x.Id));

            return leaveRequests;
        }

        public async Task<List<LeaveRequestVm>> GetbyEmployee(string Id)
        {
            var leaveRequests = await _leaveRequestRepo.GetFilteredList(
                 select: x => _mapper.Map<LeaveRequestVm>(x),
                 where: x => !x.Status.Equals(Status.Passive) && x.EmployeeId == Id,
                 orderby: x => x.OrderBy(x => x.Id));
            return leaveRequests;
        }

        public async Task<LeaveRequestVm> GetbyId(int Id)
        {
            var leaveRequest = await _leaveRequestRepo.GetDefault(l => l.Id == Id);
            if (leaveRequest == null)
            {
                return null;
            }

            var leaveRequestVm = _mapper.Map<LeaveRequestVm>(leaveRequest);
            return leaveRequestVm;
        }
        public async Task Update(UpdateLeaveRequestDto model)
        {
            //var leaveRequest = _leaveRequestRepo.GetDefault(l => l.Id == model.Id);
            var leaveRequest = _mapper.Map<LeaveRequest>(model); 
            leaveRequest.UpdateDate = DateTime.Now;
            leaveRequest.Status=Status.Modified;
            await _leaveRequestRepo.UpdateAsync(leaveRequest);
        }
    }

}
