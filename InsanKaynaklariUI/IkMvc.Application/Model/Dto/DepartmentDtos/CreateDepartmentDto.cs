using IkMvc.Application.Model.Enums;
using System.Text.Json.Serialization;

namespace IkMvc.Application.Model.Dto.DepartmentDtos
{
    public class CreateDepartmentDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [JsonPropertyName("status")]
        public Status Status { get; set; } = Status.Active;
    }
}
