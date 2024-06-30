using System.Text.Json.Serialization;

namespace IkMvc.Application.Model.Dto.UserDtos
{
    public class ResetPasswordDto
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; }

        [JsonPropertyName("confirmPassword")]
        public string ConfirmPassword { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
