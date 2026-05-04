using System.Text.Json.Serialization;

namespace StajKariyerWeb.Models
{
    public class CompanyMatch
    {
        [JsonPropertyName("FİRMA")]
        public string Firma { get; set; } = string.Empty;

        [JsonPropertyName("Ilgili_Alan")]
        public string IlgiliAlan { get; set; } = string.Empty;

        [JsonPropertyName("Score")]
        public int Score { get; set; }

        [JsonPropertyName("Durum")]
        public string Durum { get; set; } = string.Empty;
    }
}