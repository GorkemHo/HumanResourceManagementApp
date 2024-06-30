using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.AdvanceDtos;
using Ik_Bitirme.Application.Models.DTos.ExpenseRequestDtos;
using Ik_Bitirme.Application.Models.VMs.ExpenseRequestsVms;
using Ik_Bitirme.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.ExpenseRequestServices
{
    public interface IExpenseRequestService
    {
        Task<bool> CreateExpenseRequest(CreateExpenseRequestDto model);
        Task<bool> UpdateExpenseRequest(UpdateExpenseRequestDto model);
        Task<ExpenseRequest> GetById(int id);
        Task<List<ExpenseRequestVm>> GetByEmployeeId(string employeeId);
        Task<List<ExpenseRequestVm>> GetAllExpenseRequests();
        Task<bool> SoftDeleteExpenseRequest(int id);
        Task<bool> ApproveExpense(int id);
        Task<bool> RejectExpense(int id);
        Task<List<ExpenseStatusViewDto>> GetPendingList();
        Task<List<ExpenseStatusViewDto>> GetApprovedList();
        Task<List<ExpenseStatusViewDto>> GetRejectList();
    }
}
