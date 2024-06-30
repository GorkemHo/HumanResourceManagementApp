using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Domain.Entities
{
    public class Director : AppUser, IBaseEntity
    {
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
