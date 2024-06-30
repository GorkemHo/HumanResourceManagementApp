using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Models.DTos.ErrorLogDtos
{
    public class CreateErrorLogDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Location { get; set; }

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
