namespace StajKariyerWeb.Models
{
    public class PredictionRequest
    {
        public decimal GNO { get; set; }
        public string Ilgili_Alan { get; set; } = string.Empty;
        public string Proje { get; set; } = string.Empty;
        public int Veritabani { get; set; }
        public int Python { get; set; }
        public int Java { get; set; }
        public int Csharp { get; set; }
        public int Cpp { get; set; }
    }
}