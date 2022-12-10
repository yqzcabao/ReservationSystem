using Microsoft.AspNetCore.Mvc;
using ReservationApp.Services;
using ReservationApp.Data;
using ReservationApp.Models;

namespace ReservationApp.Controllers
{
    public class OrderController : Controller
    {
        ReservationAppDbContext DB;
        IProductservices IPservices;
        IFileService IFService;
        ICategoryService ICService;

        public OrderController(IFileService _IFService, ReservationAppDbContext _Db, ICategoryService _Categoryservices, IProductservices _IPservices)
        {
            ICService = _Categoryservices;
            IPservices = _IPservices;
            DB = _Db;
            IFService = _IFService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateOrder(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productFormDb = DB.Product.Find(id);
            var stockQty = DB.Stock.Find(id);
            productFormDb.Qty = stockQty.Qty;

            if (productFormDb == null)
            {
                return NotFound();
            }

            ViewData["Product"] = productFormDb;
            return View();
            //ViewData["Header"] = "Movie Details"
            //return View(productFormDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(Order Ord)
        {
            var pro= DB.Product.Find(Ord.ProductID);
            Ord.TotalPrice = Ord.Qty * pro.UnitPrice;
        

            var pStock = DB.Stock.Find(Ord.ProductID);
            pStock.Qty = pStock.Qty - Ord.Qty;

            //var stock = new Stock
            //{

            //    Qty = pStock.Qty- Ord.Qty
            //};

            DB.Order.Add(Ord);
            DB.SaveChanges();
            DB.Stock.Update(pStock);
            DB.SaveChanges();
            TempData["success"] = "Order Submitted Successfully, your order Number is " + Ord.OrderID+" Order details have been sent to your email";
            return RedirectToAction("ProductList","Product");

        }


    }
}
