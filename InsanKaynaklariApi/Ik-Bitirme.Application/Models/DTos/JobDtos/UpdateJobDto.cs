using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.DTos.JobDtos
{
    public class UpdateJobDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public DateTime? UpdateDate { get; set; }=DateTime.Now;
        
        public Status Status { get; set; }=Status.Modified;
    }
}
