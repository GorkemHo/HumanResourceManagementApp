using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Domain.Entities
{
    public class Employee : AppUser, IBaseEntity
    {
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        public List<ExpenseRequest>? Expenses { get; set; }
        public List<LeaveRequest>? Leaves { get; set; }
        public List<AdvanceRequest>? Advances { get; set; }

        public Employee()
        {
            Expenses = new List<ExpenseRequest>();
            Leaves = new List<LeaveRequest>();
            Advances = new List<AdvanceRequest>();
        }
    }
}
