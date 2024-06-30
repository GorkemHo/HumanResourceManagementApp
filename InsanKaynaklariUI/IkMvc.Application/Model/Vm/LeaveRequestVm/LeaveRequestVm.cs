using IkMvc.Application.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Vm.LeaveRequestVm
{
    public class LeaveRequestVm
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; }

        [JsonPropertyName("leaveType")]
        public string LeaveType { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; }

        [JsonPropertyName("numberOfDays")]
        public int NumberOfDays { get; set; }

        [JsonPropertyName("approvalStatus")]
        public ApprovalStatus ApprovalStatus { get; set; }

        [JsonPropertyName("responseDate")]
        public DateTime? ResponseDate { get; set; }

        [JsonIgnore]
        public string? EncryptedId { get; set; }
    }
}
