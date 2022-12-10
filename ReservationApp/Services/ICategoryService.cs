using ReservationApp.Models;

namespace ReservationApp.Services
{
    public interface ICategoryService
    {
        public bool Add(Sitting model);

        public bool Update(Sitting model);

        public Sitting GetById(int id);

        public bool Delete(int id);

        public IQueryable<Sitting> List();
    }
}
