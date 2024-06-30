using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Domain.Entities
{
    public class Admin : AppUser, IBaseEntity
    {
        public new DateTime CreateDate { get; set; }= DateTime.Now;
        public new DateTime? UpdateDate { get; set; }
        public new DateTime? DeleteDate { get; set; }
        public new Status Status { get; set; }
    }
}
