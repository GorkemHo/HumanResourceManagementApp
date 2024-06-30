using IkMvc.Application.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.ExpenseRequestDtos
{
    public class UpdateExpenseRequestDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        //public Employee? Employee { get; set; }


        public ExpenseType ExpenseType { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public DateTime? ResponseDate { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        //public DateTime RequestDate { get; set; }


        [JsonPropertyName("imageData")]
        public byte[]? ImageData { get; set; }
        [NotMapped]
        [JsonIgnore]
        public IFormFile? UploadPath { get; set; }

        public DateTime UpdateDate = DateTime.Now;
        //public Status Status = Status.Active;
    }
}
