using IkMvc.Application.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.CompanyDtos
{
    public class UpdateCompanyDto
    {
        [JsonPropertyName("companyId")]
        public int CompanyId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("mersisNo")]
        public string MersisNo { get; set; }

        [JsonPropertyName("taxNo")]
        public string TaxNo { get; set; }

        [JsonPropertyName("taxAdministration")]
        public string TaxAdministration { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("imageData")]
        public byte[]? ImageData { get; set; }

        [JsonIgnore]
        public IFormFile? UploadPath { get; set; }

        [JsonPropertyName("yearOfEstablishment")]
        public DateTime YearOfEstablishment { get; set; } = DateTime.Now;

        [JsonPropertyName("contractStartDate")]
        public DateTime ContractStartDate { get; set; } = DateTime.Now;

        [JsonPropertyName("contractEndDate")]
        public DateTime ContractEndDate { get; set; } = DateTime.Now;

        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;

        [JsonPropertyName("status")]
        public Status Status { get; set; } = Status.Modified;
    }
}
