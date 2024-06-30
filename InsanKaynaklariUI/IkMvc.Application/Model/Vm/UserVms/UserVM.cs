
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IkMvc.Application.Model.Vm.UserVms
{
    public class UserVM
    {
        

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
        
        
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("imageData")]
        public byte[]? ImageData { get; set; }

        // [PictureFileExtensionAttiribute]
        // [JsonPropertyName("uploadPath")]
        //// public IFormFile? UploadPath { get; set; }
        [JsonPropertyName("jobId")]
        public int? JobId { get; set; }
        [JsonPropertyName("departmentId")]
        public int? DepartmentId { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
        
        
        
    }
}
