using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.CompanyDtos
{
    public class CompanyDto
    {        
        [JsonPropertyName("companyId")]
        public int CompanyId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
