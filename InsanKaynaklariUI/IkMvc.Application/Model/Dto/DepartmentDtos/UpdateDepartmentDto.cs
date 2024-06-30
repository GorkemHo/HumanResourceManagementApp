using IkMvc.Application.Model.Enums;
using System.Text.Json.Serialization;

namespace IkMvc.Application.Model.Dto.DepartmentDtos
{
    public class UpdateDepartmentDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("updateDate")]
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        [JsonPropertyName("status")]
        public Status Status { get; set; } = Status.Modified;
    }
}
