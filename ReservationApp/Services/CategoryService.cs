using ReservationApp.Data;
using ReservationApp.Models;
namespace ReservationApp.Services
{
    public class CategoryService : ICategoryService

    {
        ReservationAppDbContext Dbx;
        public CategoryService(ReservationAppDbContext _Dbx)
        {
            this.Dbx = _Dbx;
        }
        public bool Add(Sitting model)
        {
            try
            {
                Dbx.Sitting.Add(model);
                Dbx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool Delete(int id)
        {
            try
            {
                Outer_Category Outer = new Outer_Category();

                
                // Accessing static method1
                // of inner class
               var data= Outer.Inner.GetCategoryById(id);
                // var data = this.GetById(id);


                if (data == null)
                    return false;
                Dbx.Sitting.Remove(data);
                Dbx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Sitting GetById(int id)
        {
            
            return Dbx.Sitting.Find(id);
        }

        public IQueryable<Sitting> List()
        {
            var data = Dbx.Sitting.AsQueryable();
            return data;
        }

        public bool Update(Sitting model)
        {
            try
            {
                Dbx.Sitting.Update(model);
                Dbx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

       
    }
}

public class Outer_Category
{
    public Inner_Category Inner { get; set; }

    // Constructor to establish link between
    // instance of Outer_class to its
    // instance of the Inner_class
    public Outer_Category()
    {
        this.Inner = new Inner_Category(this);
    }



    public class Inner_Category
    {
        ReservationAppDbContext Dbx;
        private Outer_Category obj;
        public Inner_Category(ReservationAppDbContext _Dbx)
        {
            this.Dbx = _Dbx;
           
        }
        public Inner_Category(Outer_Category outer)
        {
           
            this.obj = outer;
        }

        public Sitting GetCategoryById(int id)
        {
            return Dbx.Sitting.Find(id);
        }

    }
}

