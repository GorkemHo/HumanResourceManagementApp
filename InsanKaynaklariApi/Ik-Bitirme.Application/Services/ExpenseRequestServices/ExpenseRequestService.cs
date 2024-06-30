using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.ExpenseRequestDtos;
using Ik_Bitirme.Application.Models.VMs.ExpenseRequestsVms;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.ExpenseRequestServices
{
    public class ExpenseRequestService : IExpenseRequestService
    {
        private readonly IMapper _mapper;
        private readonly IExpenseRequestRepo _expenseRequestRepo;

        public ExpenseRequestService(IExpenseRequestRepo expenseRequestRepo, IMapper mapper)
        {
            _expenseRequestRepo = expenseRequestRepo;
            _mapper = mapper;
        }

        public async Task<bool> CreateExpenseRequest(CreateExpenseRequestDto model)
        {
            try
            {
                ExpenseRequest request = _mapper.Map<ExpenseRequest>(model);

                request.ApprovalStatus = ApprovalStatus.Pending;

                await _expenseRequestRepo.CreateAsync(request);

                return true;
            }
            catch (Exception)
            {
                throw new Exception("Kayıt oluşturulurken Hata!");
            }
        }

        public async Task<ExpenseRequest> GetById(int id)
        {
            if (id > 0)
            {
                return await _expenseRequestRepo.GetDefault(x => x.Id == id);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ExpenseRequestVm>> GetAllExpenseRequests()
        {

            List<ExpenseRequestVm> list = _mapper.Map<List<ExpenseRequestVm>>(await _expenseRequestRepo.GetFilteredList(select: x => new ExpenseRequestVm
            {
                Id = x.Id,
                Amount = x.Amount,
                ApprovalStatus = x.ApprovalStatus,
                Currency = x.Currency,
                EmployeeId = x.EmployeeId,
                ExpenseType = x.ExpenseType,
                RequestDate = x.RequestDate,
                ResponseDate = x.ResponseDate
            }, where: x => x.Status != Status.Passive));

            if (list != null)
                return list;
            else
                return null;

        }

        public async Task<bool> UpdateExpenseRequest(UpdateExpenseRequestDto model)
        {
            ExpenseRequest request = await _expenseRequestRepo.GetDefault(x => x.ApprovalStatus == ApprovalStatus.Pending && x.EmployeeId == model.EmployeeId && x.Id == model.Id);

            if (request != null)
            {
                if (model.ImageData != null)
                {
                    request.ImageData = model.ImageData;
                }

                // Map properties from model to existing tracked entity
                _mapper.Map(model, request);

                await _expenseRequestRepo.UpdateAsync(request);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteExpenseRequest(int id)
        {
            ExpenseRequest request = await _expenseRequestRepo.GetDefault(x => x.Id == id);
            if (request != null)
            {
                request.Status = Status.Passive;
                request.DeleteDate = DateTime.Now;
                await _expenseRequestRepo.UpdateAsync(request);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> ApproveExpense(int id)
        {
            ExpenseRequest expense = await _expenseRequestRepo.GetDefault(x => x.Id == id);
            if (expense != null)
            {
                expense.ApprovalStatus = ApprovalStatus.Approved;
                expense.ResponseDate = DateTime.Now;
                expense.UpdateDate = DateTime.Now;

                await _expenseRequestRepo.UpdateAsync(expense);
                return true;

            }
            else
                return false;
        }

        public async Task<bool> RejectExpense(int id)
        {
            ExpenseRequest expense = await _expenseRequestRepo.GetDefault(x => x.Id == id);
            if (expense != null)
            {
                expense.ApprovalStatus = ApprovalStatus.Rejected;
                expense.ResponseDate = DateTime.Now;
                expense.UpdateDate = DateTime.Now;

                await _expenseRequestRepo.UpdateAsync(expense);
                return true;

            }
            else
                return false;
        }

        public async Task<List<ExpenseStatusViewDto>> GetPendingList()
        {
            List<ExpenseStatusViewDto> expenseStatusViewDtos = await _expenseRequestRepo.GetFilteredList(select: x => new ExpenseStatusViewDto
            {
                Id = x.Id,
                Status = x.Status,
                ExpenseType = x.ExpenseType,
                Amount = x.Amount,
                ApprovalStatus = x.ApprovalStatus,
                CreateDate = x.CreateDate,
                Currency = x.Currency,
                EmployeeId = x.EmployeeId,
                ImageData = x.ImageData,
                RequestDate = x.RequestDate
            }, where: x => x.ApprovalStatus == ApprovalStatus.Pending && x.Status != Status.Passive, include: e => e.Include(i => i.Employee));

            return expenseStatusViewDtos;
        }

        public async Task<List<ExpenseStatusViewDto>> GetApprovedList()
        {
            List<ExpenseStatusViewDto> expenseStatusViewDtos = await _expenseRequestRepo.GetFilteredList(select: x => new ExpenseStatusViewDto
            {
                Id = x.Id,
                Status = x.Status,
                ExpenseType = x.ExpenseType,
                Amount = x.Amount,
                ApprovalStatus = x.ApprovalStatus,
                CreateDate = x.CreateDate,
                Currency = x.Currency,
                EmployeeId = x.EmployeeId,
                ImageData = x.ImageData,
                RequestDate = x.RequestDate
            }, where: x => x.ApprovalStatus == ApprovalStatus.Approved && x.Status != Status.Passive, include: e => e.Include(i => i.Employee));

            return expenseStatusViewDtos;
        }

        public async Task<List<ExpenseStatusViewDto>> GetRejectList()
        {
            List<ExpenseStatusViewDto> expenseStatusViewDtos = await _expenseRequestRepo.GetFilteredList(select: x => new ExpenseStatusViewDto
            {
                Id = x.Id,
                Status = x.Status,
                ExpenseType = x.ExpenseType,
                Amount = x.Amount,
                ApprovalStatus = x.ApprovalStatus,
                CreateDate = x.CreateDate,
                Currency = x.Currency,
                EmployeeId = x.EmployeeId,
                ImageData = x.ImageData,
                RequestDate = x.RequestDate
            }, where: x => x.ApprovalStatus == ApprovalStatus.Rejected && x.Status != Status.Passive, include: e => e.Include(i => i.Employee));

            return expenseStatusViewDtos;
        }

        public async Task<List<ExpenseRequestVm>> GetByEmployeeId(string employeeId)
        {
            var expenses = await _expenseRequestRepo.GetFilteredList(
               select: x => _mapper.Map<ExpenseRequestVm>(x),
               where: x => x.EmployeeId == employeeId && x.Status != Status.Passive,
               orderby: x => x.OrderBy(x => x)
           );
            return expenses;
        }
    }
}