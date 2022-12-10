using ReservationApp.Models;
using System.Security.Cryptography;

namespace ReservationApp.Services
{
    public interface IReservationService
    {
        public IEnumerable<Reservation> GetAllReservations();
        public void CreateReservation(Reservation R);
        
    }
}
