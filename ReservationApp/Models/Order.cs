using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationApp.Models
{
    public class Order
    {
        
        [Key]
        public int OrderID { get; set; }
        
        [Required]
        public int ProductID { get; set; }
        public DateTime OrderDate { get; set; }=new DateTime();
        public string OrderDescription { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        public Product Product { get; set; }
    }
}
