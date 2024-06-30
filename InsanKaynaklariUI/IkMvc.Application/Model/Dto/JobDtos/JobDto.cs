using IkMvc.Application.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Dto.JobDtos
{
    public class JobDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [JsonPropertyName("status")]
        public Status Status { get; set; } = Status.Active;
    }
}
