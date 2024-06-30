using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Domain.Entities;

namespace Ik_Bitirme.Application.Models.DTos.AdvanceDtos
{
    public class AdvanceDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public string Description { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
    }
}
