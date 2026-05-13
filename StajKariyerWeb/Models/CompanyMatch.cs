using System.Text.Json.Serialization;

namespace StajKariyerWeb.Models
{
    public class CompanyMatch
    {
        [JsonPropertyName("firma")]
        public string Firma { get; set; } = string.Empty;

        [JsonPropertyName("alan")]
        public string Alan { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("durum")]
        public string Durum { get; set; } = string.Empty;
    }
}