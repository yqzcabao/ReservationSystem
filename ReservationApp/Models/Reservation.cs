using System.ComponentModel;
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
        [DisplayName("Start Time")]
        public DateTime StartDateTime { get; set; }
        [Required]
        [DisplayName("Duration(Minutes)")]
        public int Duration { get; set; }// minutes
        [Required]
        [DisplayName("Number of guests")]
        public int NumberOfGuests { get; set; }
        [Required]
        [DisplayName("Status")]
        public string ReservationStatus { get; set; }

        [DisplayName("Addition Notes")]
        public string AdditionNotes { get; set; }
        [DisplayName("Booking Date")]
        public DateTime BookingDateTime { get; set; } = DateTime.Now;

        //public List<ReservationSitting> ReservationSitting { get; set; }
    }

    //Due to User Id can't directly passed from view model, so create another class for the modelstate validation.
    public class ReservationInViewModel
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        [DisplayName("Duration(Minutes)")]
        public int Duration { get; set; }// minutes
        [Required]
        [DisplayName("Number of guests")]
        public int NumberOfGuests { get; set; }

        [DisplayName("Addition Notes")]
        public string AdditionNotes { get; set; }
        public DateTime BookingDateTime { get; set; } = DateTime.Now;

        [Required]
        public string Area { get; set; }

        [Required]
        public string TableID { get; set; }
        //public List<ReservationSitting> ReservationSitting { get; set; }
    }

    public class ReservationIndex : ReservationInViewModel
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DisplayName("Status")]
        public string ReservationStatus { get; set; }
    }

    public class ReservationCreateByStaff : ReservationInViewModel 
    {
        public string RegistrationID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Phone]
        public string MobilePhone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Minimum length 6 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string? Role { get; set; }
    }
}
