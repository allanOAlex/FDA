using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TB.Domain.Models
{
    public class StockPrice
    {
        [Key]
        public int Id { get; set; }
        public string? Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal CloseAdjusted { get; set; }
        public int Volume { get; set; } 
        public decimal SplitCoefficient { get; set; }

        [NotMapped]
        public string? DataUrl { get; set; }


    }
}
