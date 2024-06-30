using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;

namespace Ik_Bitirme.Application.Models.DTos.ExpenseRequestDtos
{
    public class CreateExpenseRequestDto
    {
        //Navigation
        public string EmployeeId { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }


        public DateTime RequestDate = DateTime.Now;
        public DateTime CreateDate = DateTime.Now;
        public Status Status = Status.Active;

        public byte[]? ImageData { get; set; }
    }
}
