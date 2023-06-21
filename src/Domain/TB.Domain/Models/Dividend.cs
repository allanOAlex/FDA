using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
