using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    //This is for linking reservation and sitting table
    public class ReservationSitting
    {
        [Key]
        public int ReservationSittingId { get; set; }
        [Required]
        public int ReservationID { get; set; }
        public Reservation Reservation { get; set; }
        [Required]
        public int SittingTableID { get; set; }
        public SittingTable SittingTable { get; set; }
        

    }
}
