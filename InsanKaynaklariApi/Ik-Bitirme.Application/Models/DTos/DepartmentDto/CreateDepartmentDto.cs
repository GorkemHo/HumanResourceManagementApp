using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.DTos.DepartmentDto
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
        public Status Status { get; set; } = Status.Active;
    }
}
