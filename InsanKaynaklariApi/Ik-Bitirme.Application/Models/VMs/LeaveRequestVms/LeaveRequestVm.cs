using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.VMs.LeaveRequestVms
{
    public class LeaveRequestVm
    {
        public int Id { get; set; }        
        public string EmployeeId { get; set; }
       // public Employee Employee { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RequestDate { get; set; }
        public int NumberOfDays { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ResponseDate { get; set; }                     
        
    }
}

