using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.ErrorLogDtos
{
    public class CreateErrorLogDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
