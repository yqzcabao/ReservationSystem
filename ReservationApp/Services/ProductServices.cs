using ReservationApp.Models;
using ReservationApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ReservationApp.Services
{
    public class ProductServices : IProductservices
    {

        ReservationAppDbContext Db;
        public ProductServices(ReservationAppDbContext _Db)
        {
            Db = _Db;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return (Db.Product.Select(u => u).ToList());
        }
        public void CreateProduct(Product P)
        {

            Db.Product.Add(P);
         //  Db.Product.Attach(P);

            Db.SaveChanges();
           
            
            Db.ChangeTracker.Clear();
           
            
           
            //Db.Entry<Product>(P).State= EntityState.Detached;


        }
    }
}
