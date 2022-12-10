using ReservationApp.Models;
using ReservationApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ReservationApp.Services
{
    public class ReservationService : IReservationService
    {

        ReservationAppDbContext Db;
        public ReservationService(ReservationAppDbContext _Db)
        {
            Db = _Db;
        }
        public IEnumerable<Reservation> GetAllReservations()
        {
            return (Db.Reservation.Select(u => u).ToList());
        }
        public void CreateReservation(Reservation R)
        {

            Db.Reservation.Add(R);
         //  Db.Reservation.Attach(P);

            Db.SaveChanges();
           
            
            Db.ChangeTracker.Clear();
           
            
           
            //Db.Entry<Reservation>(R).State= EntityState.Detached;


        }
    }
}
