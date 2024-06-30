using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.DTos.DirectorDtos
{
    public class DirectorDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string TcIdentity { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public byte[]? ImageData { get; set; }
        public int? JobId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
        public int? DepartmentId { get; set; }
        public int? CompanyId { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

    }
}
