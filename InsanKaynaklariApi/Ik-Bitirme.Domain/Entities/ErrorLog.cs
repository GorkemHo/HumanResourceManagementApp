using Ik_Bitirme.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Domain.Entities
{
    public class ErrorLog: IBaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Location { get; set; }

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }
        public DateTime CreateDate { get; set ; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
    }
}
