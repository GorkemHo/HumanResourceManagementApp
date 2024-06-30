using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.VMs.UserVMs
{
    public class UserVM
    {              
        public string? Email { get; set; }
        
        public string FirstName { get; set; }
        
        public string? MiddleName { get; set; }
        
        public string LastName { get; set; }
        
        public string? SecondLastName { get; set; }
               
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }

        public byte[]? ImageData { get; set; }
        public string Role { get; set; }
       
        public int? JobId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
