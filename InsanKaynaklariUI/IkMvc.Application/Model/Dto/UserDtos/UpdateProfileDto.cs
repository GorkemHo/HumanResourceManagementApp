using Ik_Bitirme.Application.Extensions;
using IkMvc.Application.Model.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Ik_Bitirme.Application.Models.DTos.UserDtos
{
    public class UpdateProfileDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [JsonPropertyName("confirmPassword")]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        [JsonPropertyName("email")]
        public string? Email { get; set; }
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
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Identity number must be 11 digits")]
        [JsonPropertyName("tcIdentity")]
        public string TcIdentity { get; set; }
        [JsonPropertyName("hireDate")]
        public DateTime HireDate { get; set; }
        [JsonPropertyName("terminationDate")]
        public DateTime? TerminationDate { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits")]
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [MaxLength(250, ErrorMessage = "Address must be maximum 250 characters")]
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [Range(0, 200000, ErrorMessage = "Salary must be a positive number. 0-200.000")]
        [JsonPropertyName("salary")]
        public decimal Salary { get; set; }
        [JsonPropertyName("imageData")]
        public byte[]? ImageData { get; set; }

        [JsonPropertyName("jobId")]
        public int? JobId { get; set; }
        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
        [JsonPropertyName("status")]
        public Status Status = Status.Modified;
        [JsonPropertyName("departmentId")]
        public int? DepartmentId { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("companyId")]
        public int? CompanyId { get; set; }
    }
}
