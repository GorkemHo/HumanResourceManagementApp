using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.VMs.JobVMs
{
    public class JobVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }=Status.Active;
         
    }
}
