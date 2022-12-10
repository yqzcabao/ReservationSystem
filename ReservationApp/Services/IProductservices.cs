using ReservationApp.Models;
using System.Security.Cryptography;

namespace ReservationApp.Services
{
    public interface IProductservices
    {
        public IEnumerable<Product> GetAllProducts();
        public void CreateProduct(Product P);
        
    }
}
