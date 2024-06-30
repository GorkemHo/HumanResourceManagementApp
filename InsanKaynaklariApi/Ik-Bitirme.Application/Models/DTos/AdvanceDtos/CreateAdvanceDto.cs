using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.DTos.AdvanceDtos
{
    public class CreateAdvanceDto
    {
        public string EmployeeId { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public string Description { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime CreateDate { get; set; } =DateTime.Now;
        public Status Status { get; set; } = Status.Active;
    }
}
