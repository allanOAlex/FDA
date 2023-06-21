using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TB.Shared.Requests.Common;
using System.Text.Json.Serialization;

namespace TB.Shared.Requests.Auth
{
    public record LoginRequest : Request
    {
        [JsonPropertyName("UserName")]
        [Required(ErrorMessage = "Username is required"), MinLength(3, ErrorMessage = "Username must have a minimum of 3 characters")]
        public string? UserName { get; set; }

        [JsonPropertyName("Password")]
        public string? Password { get; set; }

        [JsonPropertyName("RememberMe")]
        [DefaultValue(false)]
        public bool RememberMe { get; set; } = false;
    }
}
