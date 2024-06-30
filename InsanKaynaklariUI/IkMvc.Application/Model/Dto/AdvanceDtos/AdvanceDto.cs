using IkMvc.Application.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.AdvanceDtos
{
    public class AdvanceDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; }
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonPropertyName("currency")]
        public CurrencyType Currency { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("approvalStatus")]
        public ApprovalStatus? ApprovalStatus { get; set; }
        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; }
        [JsonPropertyName("responseDate")]
        public DateTime? ResponseDate { get; set; }
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }
        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; }
        [JsonPropertyName("deleteDate")]
        public DateTime? DeleteDate { get; set; }
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonIgnore]
        public string? EncryptedId { get; set; }
    }
}
