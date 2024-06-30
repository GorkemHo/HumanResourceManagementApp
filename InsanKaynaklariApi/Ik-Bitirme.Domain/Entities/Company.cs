using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Domain.Entities
{
    public class Company : IBaseEntity
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public int? EmployeesCount { get; set; }
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

        public List<Employee>? Employees { get; set; }

        public List<Director>? Directors { get; set; }

        public List<Department>? Departments { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
    }
}
