using Ik_Bitirme.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Domain.Entities
{
    public class AppUser : IdentityUser, IBaseEntity
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string TcIdentity { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public byte[]? ImageData { get; set; }
        [NotMapped]
        public IFormFile? UploadPath { get; set; }

        public int? JobId { get; set; }
        public Job Job { get; set; }
        public DateTime CreateDate { get ; set ; }
        public DateTime? UpdateDate { get ; set; }
        public DateTime? DeleteDate { get ; set; }
        public Status Status { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
