using IkMvc.Application.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.DTos.UserDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("middleName")]
        public string? MiddleName { get; set; }

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

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits")]
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [MaxLength(250, ErrorMessage = "Address must be maximum 250 characters")]
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [Range(0, 200000, ErrorMessage = "Salary must be a positive number. 0-200.000")]
        [JsonPropertyName("salary")]
        public decimal Salary { get; set; }

        [JsonPropertyName("email")]
        [Required(ErrorMessage = "Email address is required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please check your email address and enter a valid one")]
        public string Email { get; set; }

        [JsonPropertyName("password")]

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password, ErrorMessage = "Your password does not meet the required criteria")]
        public string Password { get; set; }


        [JsonPropertyName("confirmPassword")]
        [Required(ErrorMessage = "Password confirmation is required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }

        [JsonPropertyName("uploadPath")]

        [NotMapped]
        public IFormFile? UploadPath { get; set; }

        [JsonPropertyName("jobId")]
        public int? JobId { get; set; }

        [JsonPropertyName("status")]
        public Status Status => Status.Active;

        [JsonPropertyName("createDate")]
        public DateTime CreateDate => DateTime.Now;
    }
}
