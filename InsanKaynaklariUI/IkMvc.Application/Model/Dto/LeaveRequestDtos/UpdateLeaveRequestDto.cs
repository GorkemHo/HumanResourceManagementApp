using IkMvc.Application.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.LeaveRequestDtos
{
    public class UpdateLeaveRequestDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RequestDate { get; set; }
        public int NumberOfDays { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}
