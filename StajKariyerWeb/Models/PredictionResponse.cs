using System.Text.Json.Serialization;

namespace StajKariyerWeb.Models
{
    public class PredictionResponse
    {
        [JsonPropertyName("prediction")]
        public string Prediction { get; set; } = string.Empty;

        [JsonPropertyName("matched_area")]
        public string MatchedArea { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("companies")]
        public List<CompanyMatch> Companies { get; set; } = new();
    }
}