using System.Text.Json.Serialization;

namespace StajKariyerWeb.Models
{
    public class PredictionResponse
    {
        [JsonPropertyName("tahmin")]
        public string Tahmin { get; set; } = string.Empty;

        [JsonPropertyName("firma_eslesme_alani")]
        public string FirmaEslesmeAlani { get; set; } = string.Empty;

        [JsonPropertyName("onerilen_firmalar")]
        public List<CompanyMatch> OnerilenFirmalar { get; set; } = new();
    }
}