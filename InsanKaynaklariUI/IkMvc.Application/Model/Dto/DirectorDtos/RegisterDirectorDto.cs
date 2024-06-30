using IkMvc.Application.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.DirectorDtos
{
    public class RegisterDirectorDto
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please check your email address and enter a valid one")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password, ErrorMessage = "Your password does not meet the required criteria")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? SecondLastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Identity number must be 11 digits")]
        public string TcIdentity { get; set; }
        public DateTime HireDate { get; set; }
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits")]
        public string PhoneNumber { get; set; }
        [MaxLength(250, ErrorMessage = "Address must be maximum 250 characters")]
        public string Address { get; set; }
        [Range(0, 200000, ErrorMessage = "Salary must be a positive number. 0-200.000")]
        public decimal Salary { get; set; }
        [JsonIgnore]
        public IFormFile? UploadPath { get; set; }
        public byte[]? ImageData { get; set; }
        public int? JobId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Active;
        public int? DepartmentId { get; set; }
        public int? CompanyId { get; set; }
    }
}
