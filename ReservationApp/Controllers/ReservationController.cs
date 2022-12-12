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

namespace ReservationApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationController(ReservationAppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: all reservation
        public async Task<IActionResult> Index()
        {
            var reservationIndexList = GetAllReservationCombine();
            return View(reservationIndexList);
        }

        // GET: reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservationIndexList = GetAllReservationCombine();
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
            .ToList();

            var TableIdList = query.Select(a => new SelectListItem { Text = a.Table, Value = a.SittingTableId.ToString() });
            return Json(TableIdList);
        }

        // POST: reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //public async Task<IActionResult> Create([Bind("ReservationId,UserID,StartDateTime,Duration,NumberOfGuests,ReservationStatus,AdditionNotes,BookingDateTime")] Reservation reservation)
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

            var reservationIndexList = GetAllReservationCombine();
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
                        rs.SittingTableID = int.Parse(reservationIndex.TableID);
                        _context.SaveChanges();
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
        private List<ReservationIndex> GetAllReservationCombine(){
            var users = _userManager.Users.ToList();
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
                foreach (var u in users)
                {
                    if (u.Id == r.UserID)
                    {
                        reservationIndex.FirstName = u.FirstName;
                        reservationIndex.LastName = u.LastName;
                    }
                }
                reservationIndexList.Add(reservationIndex);
            }

            return reservationIndexList;
        }
    }
}
