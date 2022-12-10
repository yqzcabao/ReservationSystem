using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    //This is for the sitting table area, table id and table capacity
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        [Required]
        public string UserID { get; set; }// UserID should get from Aspnet Core Identity
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public int Duration { get; set; }// minutes
        [Required]
        public int NumberOfGuests { get; set; }
        [Required]
        public string ReservationStatus { get; set; }

        public string AdditionNotes { get; set; }
        public DateTime BookingDateTime { get; set; } = new DateTime();

        //public List<ReservationSitting> ReservationSitting { get; set; }
    }
}
