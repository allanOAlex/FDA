using System.ComponentModel.DataAnnotations;

namespace TB.Domain.Models
{
    public class Dividend
    {
        public Dividend()
        {
                
        }

        [Key]
        public int Id { get; set; }
        public string? Symbol { get; set; }
        public decimal Dividends { get; set; } 



    }
}
