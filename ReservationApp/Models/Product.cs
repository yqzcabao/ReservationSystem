using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationApp.Models
{
    public class Product
    {
        [Key]
        public int ProuctId { get; set; }
        [Required]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public double UnitPrice { get; set; }

        public string? Image { get; set; }
        public int CategoryID { get; set; }
        
        public Sitting Category { get; set; }
        public Stock Stock { get; set; }
        public List<Order> Order { get; set; } = new List<Order>();
        

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        [Required]
        // public List<int>? Categories { get; set; }
        public int Categories { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        [NotMapped]
        public string? CategoryNames { get; set; }
        [NotMapped]
        public int? Qty { get; set; }
    }
}
