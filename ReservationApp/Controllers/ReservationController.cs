using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Models;
using ReservationApp.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;

namespace ReservationApp.Controllers
{
   
    public class ReservationController : Controller
    {
        ReservationAppDbContext DB;
        IProductservices IPservices;
        IFileService IFService;
        ISittingService ICService;

        public ReservationController(IFileService _IFService, ReservationAppDbContext _Db, ISittingService _Categoryservices, IProductservices _IPservices)
        {
            ICService= _Categoryservices;
            IPservices = _IPservices;
            DB = _Db;
            IFService = _IFService;
        }

        
        public IActionResult Index()
        {
          return View(IPservices.GetAllProducts());
        }

        public IActionResult ProductList()
        {
            return View(IPservices.GetAllProducts());
        }

         
        
        // display create view
        [Authorize(Roles ="admin")]
        public IActionResult Create()
        {
            var model = new Product();
            model.CategoryList = ICService.List().Select(a => new SelectListItem { Text = a.SittingName, Value = a.SittingId.ToString()});
            return View(model);
          
          
        }

        // save new Product
        //post
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product Pobj)
        {
            
            Pobj.CategoryID = Pobj.Categories;
           

            if (Pobj.ImageFile != null)
            {
                var fileReult = this.IFService.SaveImage(Pobj.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(Pobj);
                }
                var imageName = fileReult.Item2;
                Pobj.Image = imageName;
            }


            IPservices.CreateProduct(Pobj);
            var stock = new Stock
            {
                ProductId = Pobj.ProuctId,
                Qty = 0
            };
            DB.Stock.Add(stock);
            DB.SaveChanges();
            TempData["success"] = "Product Created Successfully";
            return RedirectToAction("Index");
            

        }

        // display Edit view
       [Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {
           
        //  DB.Entry<Product>.State = EntityState.Added;
            if (id==null || id==0)
            {
                return NotFound();
            }
            
            var CategoryFormDb = DB.Product.Find(id);
            var x = CategoryFormDb.Image;
            if (CategoryFormDb==null)
            {
                return NotFound();
            }
            CategoryFormDb.CategoryList = ICService.List().Select(a => new SelectListItem { Text = a.SittingName, Value = a.SittingId.ToString() });


            return View(CategoryFormDb);
        }

        // Update Product
        //post
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Product Pobj)
        {

            Pobj.CategoryID = Pobj.Categories;

            if (Pobj.ImageFile != null)
            {
                var fileReult = this.IFService.SaveImage(Pobj.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(Pobj);
                }
                var imageName = fileReult.Item2;
                Pobj.Image = imageName;
            }

            using (var transaction = DB.Database.BeginTransaction())
            {
                try
                {
                    /*do something*/
                    DB.Product.Update(Pobj);
                    //   DB.SaveChanges();



                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    DB.Remove(Pobj);
                    transaction.Rollback();
                }
            }

          //  DB.Product.Update(Pobj);
        //    DB.SaveChanges();

            //  DB.Remove(Pobj);

            TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
 

            //}
            //return View(Pobj);
        }

        // display Delete view

        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var CategoryFormDb = DB.Product.Find(id);
            if (CategoryFormDb == null)
            {
                return NotFound();
            }
            return View(CategoryFormDb);
        }

        // Delete Product
        //post
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {

            Product product = DB.Product.FirstOrDefault(s => s.ProuctId == Id);
            if (product !=null)
            {
                DB.Remove(product);
                DB.SaveChanges();
                TempData["success"] = "Product Deleted Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        
    }
}
