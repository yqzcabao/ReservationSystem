using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReservationApp.Data;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _authService;

        public ReservationController(ReservationAppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IUserService authService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        // GET: all reservation
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)//user login in
            {
                //System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                //var userId = _userManager.GetUserId(currentUser);
                
                
                var reservationIndexList = GetAllReservationCombine(User);
                return View(reservationIndexList);
            }
            else// user not login in
            {
                return Redirect("/User/Login");                
            }

        }

        // GET: reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
                        
            if (id == null || _context.Reservation.Where(m => m.ReservationId == id).Count() == 0)
            {
                return NotFound();
            }

            var reservationIndexList = GetAllReservationCombine(User);
            ReservationIndex reservation = new ReservationIndex();

            foreach(var r in reservationIndexList)
            {
                if(r.ReservationId == id)
                {
                    reservation = r;
                }
            }

            return View(reservation);
        }

        // GET: reservation/Create
        public IActionResult Create()
        {
            //get the table area and table ID.
            //ViewBag.TableArea = _context.SittingTable.ToList().Select(a => new SelectListItem { Text = a.Area, Value = a.SittingTableId.ToString() });

            //create a group by query to select table area https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators
            var query = from s in _context.Set<SittingTable>()
                        group s by s.Area
                        into g
                        select new { g.Key, Count = g.Count() };
            //create a select list of table area
            ViewBag.TableArea = query.Select(a => new SelectListItem { Text = a.Key, Value = a.Key });
            return View();
        }

        public IActionResult GetTableIdByArea(string area)
        {
            var query = _context.SittingTable
            .Where(b => b.Area.Equals(area))
            .OrderBy(c => c.Table)
            .ToList();

            var TableIdList = query.Select(a => new SelectListItem { Text = a.Table, Value = a.SittingTableId.ToString() });
            return Json(TableIdList);
        }

        // POST: reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationInViewModel reservationInViewModel)
        {
            Reservation reservation = new Reservation();
            reservation.ReservationId = reservationInViewModel.ReservationId;
            reservation.StartDateTime = reservationInViewModel.StartDateTime;
            reservation.Duration = reservationInViewModel.Duration;
            reservation.NumberOfGuests = reservationInViewModel.NumberOfGuests;
            reservation.AdditionNotes = reservationInViewModel.AdditionNotes;
            reservation.BookingDateTime = reservationInViewModel.BookingDateTime;
            reservation.ReservationStatus = "Pending";
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            reservation.UserID = _userManager.GetUserId(currentUser);


            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                ReservationSitting reservationSitting = new ReservationSitting();
                reservationSitting.ReservationID = reservation.ReservationId;
                reservationSitting.SittingTableID = int.Parse(reservationInViewModel.TableID);
                _context.Add(reservationSitting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: reservation/CreateByStaff
        public IActionResult CreateByStaff()
        {
            //get the table area and table ID.
            //ViewBag.TableArea = _context.SittingTable.ToList().Select(a => new SelectListItem { Text = a.Area, Value = a.SittingTableId.ToString() });

            //create a group by query to select table area https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators
            var query = from s in _context.Set<SittingTable>()
                        group s by s.Area
                        into g
                        select new { g.Key, Count = g.Count() };
            //create a select list of table area
            ViewBag.TableArea = query.Select(a => new SelectListItem { Text = a.Key, Value = a.Key });
            return View();
        }

        // POST: reservation/CreateByStaff
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //add a reservation by staff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByStaff(ReservationCreateByStaff reservationCreateByStaff)
        {

            RegistrationModel userModel = new RegistrationModel();
            userModel.Role = "member";
            userModel.FirstName = reservationCreateByStaff.FirstName;
            userModel.LastName = reservationCreateByStaff.LastName;
            userModel.Email = reservationCreateByStaff.Email;
            userModel.MobilePhone = reservationCreateByStaff.MobilePhone;
            userModel.Password = reservationCreateByStaff.Password;
            userModel.PasswordConfirm = reservationCreateByStaff.PasswordConfirm;
            userModel.UserName = reservationCreateByStaff.UserName;
            userModel.RegistrationID = reservationCreateByStaff.RegistrationID;
            

            Reservation reservation = new Reservation();
            reservation.ReservationId = reservationCreateByStaff.ReservationId;
            reservation.StartDateTime = reservationCreateByStaff.StartDateTime;
            reservation.Duration = reservationCreateByStaff.Duration;
            reservation.NumberOfGuests = reservationCreateByStaff.NumberOfGuests;
            reservation.AdditionNotes = reservationCreateByStaff.AdditionNotes;
            reservation.BookingDateTime = reservationCreateByStaff.BookingDateTime;
            reservation.ReservationStatus = "Pending";
            //System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            //reservation.UserID = _userManager.GetUserId(currentUser);


            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(userModel);

                if(result.StatusCode == 1)//add user success
                {
                    var lastInsertUser = _userManager.Users.First(u => u.Email == userModel.Email);
                    reservation.UserID = lastInsertUser.Id;
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    ReservationSitting reservationSitting = new ReservationSitting();
                    reservationSitting.ReservationID = reservation.ReservationId;
                    reservationSitting.SittingTableID = int.Parse(reservationCreateByStaff.TableID);
                    _context.Add(reservationSitting);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(reservation);
                }
            }
            return View(reservation);
        }



        // GET: reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var rr = await _context.Reservation.FindAsync(id);
            if (rr == null)
            {
                return NotFound();
            }

            var reservationIndexList = GetAllReservationCombine(User);
            ReservationIndex reservation = new ReservationIndex();

            foreach (var r in reservationIndexList)
            {
                if (r.ReservationId == id)
                {
                    reservation = r;
                }
            }

            //create a group by query to select table area https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators
            var query = from s in _context.Set<SittingTable>()
                        group s by s.Area
                        into g
                        select new { g.Key, Count = g.Count() };
            //create a select list of table area
            ViewBag.TableArea = query.Select(a => new SelectListItem { Text = a.Key, Value = a.Key }) ;

            ViewBag.Status = new List<string>() { "Pending", "Confirmed", "Progressing", "Completed" };
            
            

            return View(reservation);
        }

        // POST: reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationIndex reservationIndex)
        {
            if (id != reservationIndex.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Reservation reservation = new Reservation();
                    reservation.ReservationId = reservationIndex.ReservationId;
                    reservation.UserID = reservationIndex.UserID;
                    reservation.StartDateTime = reservationIndex.StartDateTime;
                    reservation.Duration = reservationIndex.Duration;
                    reservation.NumberOfGuests = reservationIndex.NumberOfGuests;
                    reservation.AdditionNotes = reservationIndex.AdditionNotes;
                    reservation.ReservationStatus = reservationIndex.ReservationStatus;
                    
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    var rs = _context.ReservationSitting.FirstOrDefault(item => item.ReservationID == reservation.ReservationId);
                    if(rs != null)
                    {
                        if (reservationIndex.TableID != "Microsoft.AspNetCore.Mvc.ViewFeatures.StringHtmlContent")//not change the tableid
                        {
                            rs.SittingTableID = int.Parse(reservationIndex.TableID);
                            _context.SaveChanges();
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservationIndex.ReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservationIndex);
        }

        // GET: reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'ReservationAppDbContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }

            var reservationSitting = await _context.ReservationSitting.FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservationSitting != null)
            {
                _context.ReservationSitting.Remove(reservationSitting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReservationId == id);
        }

        //Get all reservation and combined with users and sitting tables
        private List<ReservationIndex> GetAllReservationCombine(System.Security.Claims.ClaimsPrincipal user){
            List<ApplicationUser> userList = new List<ApplicationUser>();

            bool isrool = user.IsInRole("admin");
            bool isstaff = user.IsInRole("staff");

            if (user.IsInRole("member"))
            {
                var users = _userManager.Users.Where(u => u.Id == _userManager.GetUserId(user)).ToList();
                foreach(var u in users)
                {
                    userList.Add(u);
                }
            }
            else
            {
                 var users = _userManager.Users.ToList();
                foreach (var u in users)
                {
                    userList.Add(u);
                }

            }
            var reservation = _context.Reservation.ToList();
            var sittingTable = _context.SittingTable.ToList();
            var reservationSitting = _context.ReservationSitting.ToList();

            List<ReservationIndex> reservationIndexList = new List<ReservationIndex>();
            foreach(var r in reservation)
            {
                ReservationIndex reservationIndex = new ReservationIndex();
                reservationIndex.ReservationId = r.ReservationId;
                reservationIndex.UserID = r.UserID;
                reservationIndex.StartDateTime = r.StartDateTime;
                reservationIndex.Duration = r.Duration;
                reservationIndex.NumberOfGuests = r.NumberOfGuests;
                reservationIndex.AdditionNotes = r.AdditionNotes;
                reservationIndex.BookingDateTime = r.BookingDateTime;
                reservationIndex.ReservationStatus = r.ReservationStatus;
                foreach(var rs in reservationSitting)
                {
                    if(rs.ReservationID == r.ReservationId)
                    {
                        foreach(var s in sittingTable)
                        {
                            if(rs.SittingTableID == s.SittingTableId)
                            {
                                reservationIndex.Area = s.Area;
                                reservationIndex.TableID = s.Table;
                            }
                        }
                    }
                }
                foreach (var u in userList)
                {
                    if (u.Id == r.UserID)
                    {
                        reservationIndex.FirstName = u.FirstName;
                        reservationIndex.LastName = u.LastName;
                        reservationIndexList.Add(reservationIndex);
                    }
                }
                
            }

            return reservationIndexList;
        }
    }
}
