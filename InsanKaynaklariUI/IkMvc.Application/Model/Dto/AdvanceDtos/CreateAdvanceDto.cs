using IkMvc.Application.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.AdvanceDtos
{
    public class CreateAdvanceDto
    {
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; }
        [JsonPropertyName("amount")]
        [Range(1, 250000, ErrorMessage = "Amount must be a positive value.1-250.000")]
        public double Amount { get; set; }
        [JsonPropertyName("currency")]
        [Required(ErrorMessage = "Currency is required.")]
        public CurrencyType Currency { get; set; }
        [JsonPropertyName("description")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }
        [JsonPropertyName("approvalStatus")]
        public ApprovalStatus? ApprovalStatus { get; set; } = Enums.ApprovalStatus.Pending;
        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; } = DateTime.Now;
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [JsonPropertyName("status")]
        public Status Status { get; set; } = Status.Active;
    }
}
