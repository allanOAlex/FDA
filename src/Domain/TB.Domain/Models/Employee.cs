using System.ComponentModel.DataAnnotations;

namespace TB.Domain.Models
{
    public class Employee
    {
        public Employee()
        {
                
        }

        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Salary { get; set; }



    }
}
