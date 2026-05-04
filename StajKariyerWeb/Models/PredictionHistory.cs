using System;
using System.ComponentModel.DataAnnotations;

namespace StajKariyerWeb.Models
{
    public class PredictionHistory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string? UserId { get; set; }
        
        public DateTime TahminTarihi { get; set; }
        
        public string? GNO { get; set; }
        public string? IlgiliAlan { get; set; }
        public string? Proje { get; set; }
        
        // Sonuçlar
        public string? Prediction { get; set; }
        public string? MatchedArea { get; set; }
    }
}
