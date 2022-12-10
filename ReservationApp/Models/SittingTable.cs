using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    //This is for the sitting table area, table id and table capacity
    public class SittingTable
    {
        [Key]
        public int SittingTableId { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public string Table { get; set; }
        [Required]
        public int Capacity { get; set; }
        public List<ReservationSitting> ReservationSitting { get; set; }

    }
}
