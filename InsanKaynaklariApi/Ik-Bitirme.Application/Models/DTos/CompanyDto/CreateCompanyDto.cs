using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.DTos.CompanyDto
{
    public class CreateCompanyDto
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string MersisNo { get; set; }
        public string TaxNo { get; set; }
        public string TaxAdministration { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public byte[]? ImageData { get; set; }
        public DateTime YearOfEstablishment { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Active;
    }
}
