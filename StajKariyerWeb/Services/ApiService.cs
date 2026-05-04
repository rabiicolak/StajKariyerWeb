using System.Text;
using System.Text.Json;
using StajKariyerWeb.Models;

namespace StajKariyerWeb.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PredictionResponse?> GetPredictionAsync(PredictionRequest request)
        {
            var apiObject = new Dictionary<string, object>
            {
                { "GNO", request.GNO },
                { "Ilgili_Alan", request.Ilgili_Alan },
                { "Proje", request.Proje },
                { "Veritabani", request.Veritabani },
                { "Python", request.Python },
                { "Java", request.Java },
                { "Csharp", request.Csharp },
                { "C++", request.Cpp }
            };

            var json = JsonSerializer.Serialize(apiObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://127.0.0.1:8002/predict", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PredictionResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}