using IkMvc.Application.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.ExpenseRequestDtos
{
    public class CreateExpenseRequestDto
    {
        

        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; }

        [JsonPropertyName("expenseType")]
        public ExpenseType ExpenseType { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public CurrencyType Currency { get; set; }

        [JsonIgnore]
        public ApprovalStatus ApprovalStatus { get; set; }

        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [JsonPropertyName("status")]
        public Status Status { get; set; } = Status.Active;

        [JsonPropertyName("imageData")]
        public byte[]? ImageData { get; set; }

        [JsonIgnore]
        public IFormFile? UploadPath { get; set; }
    }
}
