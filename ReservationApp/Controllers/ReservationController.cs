using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Models;

namespace ReservationApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationAppDbContext _context;

        public ReservationController(ReservationAppDbContext context)
        {
            _context = context;
        }

        // GET: all reservation
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sitting.ToListAsync());
        }

        // GET: reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sitting == null)
            {
                return NotFound();
            }

            var category = await _context.Sitting
                .FirstOrDefaultAsync(m => m.SittingId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: reservation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("SittingID,SittingName,SittingStartTime,SittingEndTime,SittingDescription")] Sitting category)
        public async Task<IActionResult> Create(Sitting category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sitting == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sitting.FindAsync(id);
            if (sitting == null)
            {
                return NotFound();
            }
            return View(sitting);
        }

        // POST: reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SittingId,SittingName,SittingStartTime,SittingEndTime,SittingDescription")] Sitting category)
        {
            if (id != category.SittingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.SittingId))
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
            return View(category);
        }

        // GET: reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sitting == null)
            {
                return NotFound();
            }

            var category = await _context.Sitting
                .FirstOrDefaultAsync(m => m.SittingId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sitting == null)
            {
                return Problem("Entity set 'ReservationAppDbContext.Category'  is null.");
            }
            var category = await _context.Sitting.FindAsync(id);
            if (category != null)
            {
                _context.Sitting.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Sitting.Any(e => e.SittingId == id);
        }
    }
}
