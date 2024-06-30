
using IkMvc.Application.Model.Enums;

using System.Text.Json.Serialization;

namespace IkMvc.Application.Model.Vm.ExpenseRequestVm
{
    public class ExpenseRequestVm
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; }

        [JsonPropertyName("expenseType")]
        public ExpenseType ExpenseType { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public CurrencyType Currency { get; set; }

        [JsonPropertyName("responseDate")]
        public DateTime? ResponseDate { get; set; }

        [JsonPropertyName("approvalStatus")]
        public ApprovalStatus ApprovalStatus { get; set; }

        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; }

        [JsonPropertyName("imageData")]
        public byte[]? ImageData { get; set; }

        [JsonIgnore]
        public string? EncryptedId { get; set; }
    }
}
