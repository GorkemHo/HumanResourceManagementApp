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
    public class UpdateAdvanceDtos
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("employeeId")]
        public string? EmployeeId { get; set; }
        [JsonPropertyName("amount")]
        [Required(ErrorMessage = "Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public double Amount { get; set; }
        [JsonPropertyName("currency")]
        [Required(ErrorMessage = "Currency is required.")]
        public CurrencyType Currency { get; set; }
        [JsonPropertyName("description")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string Description { get; set; }
        [JsonPropertyName("approvalStatus")]
        public ApprovalStatus? ApprovalStatus { get; set; }
        [JsonPropertyName("requestDate")]
        public DateTime? RequestDate { get; set; }
        [JsonPropertyName("responseDate")]
        public DateTime? ResponseDate { get; set; }
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }
        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
        [JsonPropertyName("status")]
        public Status Status { get; set; } = Status.Modified;
    }
}
