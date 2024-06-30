using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ik_Bitirme.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Ik_Bitirme.Domain.Entities
{
    public class ExpenseRequest : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public ExpenseType ExpenseType { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public byte[]? ImageData { get; set; }

        [NotMapped]
        public IFormFile? UploadPath { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
    }
}
