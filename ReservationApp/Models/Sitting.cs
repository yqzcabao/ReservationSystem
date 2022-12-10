using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    //This is for sitting, such as breakfast, lunch, dinner
    public class Sitting
    {
        [Key]
        public int SittingId { get; set; }
        [Required]
        public string SittingName { get; set; }
        [Required]
        public TimeOnly SittingStartTime { get; set; }
        [Required]
        public TimeOnly SittingEndTime { get; set; }
        public string? SittingDescription { get; set; }
        
        //public List<Product> Product { get; set; } = new List<Product>();
    }
}
