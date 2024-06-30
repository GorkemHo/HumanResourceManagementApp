using IkMvc.Application.Model.Enums;
using System.Text.Json.Serialization;

namespace IkMvc.Application.Model.Dto.DepartmentDtos
{
    public class DepartmentDto
    {
        [JsonPropertyName("departmentId")]
        public int DepartmentId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }
        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; }
        [JsonPropertyName("deleteDate")]
        public DateTime? DeleteDate { get; set; }
        [JsonPropertyName("status")]
        public Status Status { get; set; }
    }
}
