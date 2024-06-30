using IkMvc.Application.Model.Dto.DepartmanDtos;
using IkMvc.Application.Model.Dto.DirectorDtos;
using IkMvc.Application.Model.Dto.EmployeeDto;
using IkMvc.Application.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Vm.CompanyVm
{
    public class CompanyVm
    {
        [JsonPropertyName("companyId")]
        public int CompanyId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("employeesCount")]
        public int? EmployeesCount { get; set; }
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
        public string Email { get; set; }
        [JsonPropertyName("imageData")]
        public byte[]? ImageData { get; set; }
        [JsonPropertyName("yearOfEstablishment")]
        public DateTime YearOfEstablishment { get; set; }
        [JsonPropertyName("contractStartDate")]
        public DateTime ContractStartDate { get; set; }
        [JsonPropertyName("contractEndDate")]
        public DateTime ContractEndDate { get; set; }
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }
        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; }
        [JsonPropertyName("deleteDate")]
        public DateTime? DeleteDate { get; set; }
        [JsonPropertyName("status")]
        public Status Status { get; set; }
        [JsonPropertyName("employees")]
        public List<EmployeeDto> Employees { get; set; }
        [JsonPropertyName("directors")]
        public List<DirectorDto> Directors { get; set; }
        [JsonPropertyName("departments")]
        public List<DepartmanDto> Departments { get; set; }

        
        [JsonIgnore]
        public string? EncryptedId { get; set; }
    }
}
