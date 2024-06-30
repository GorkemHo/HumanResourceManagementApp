using IkMvc.Application.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.AdminDtos
{
    public class AdminDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("middleName")]
        public string? MiddleName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("secondLastName")]
        public string? SecondLastName { get; set; }

        [JsonPropertyName("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("birthPlace")]
        public string BirthPlace { get; set; }

        [JsonPropertyName("tcIdentity")]
        public string TcIdentity { get; set; }

        [JsonPropertyName("hireDate")]
        public DateTime HireDate { get; set; }

        [JsonPropertyName("terminationDate")]
        public DateTime? TerminationDate { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("salary")]
        public decimal Salary { get; set; }

        [JsonPropertyName("imageData")]
        public byte[]? ImageData { get; set; }

        [JsonPropertyName("jobId")]
        public int? JobId { get; set; }

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; }

        [JsonPropertyName("deleteDate")]
        public DateTime? DeleteDate { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("departmentId")]
        public int? DepartmentId { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("departmentName")]
        public string DepartmentName { get; set; }
        [JsonPropertyName("jobName")]
        public string JobName { get; set; }
    }
}
