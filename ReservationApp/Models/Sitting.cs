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
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime SittingStartTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime SittingEndTime { get; set; }
        public string? SittingDescription { get; set; }
        
        //public List<Product> Product { get; set; } = new List<Product>();
    }
}
