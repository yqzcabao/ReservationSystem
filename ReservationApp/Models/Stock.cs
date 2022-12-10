using MessagePack;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class Stock
    {

        [System.ComponentModel.DataAnnotations.Key]
        public int ProductId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public int Qty { get; set; }
        public DateTime LastUpdatedDate { get; set; }= DateTime.Now;
        
        public Product Product{ get; set; }
    }
}
