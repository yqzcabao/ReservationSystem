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
    public class SittingController : Controller
    {
        private readonly ReservationAppDbContext _context;

        public SittingController(ReservationAppDbContext context)
        {
            _context = context;
        }

        // GET: sitting
        public async Task<IActionResult> Index()
        {
              return View(await _context.Sitting.ToListAsync());
        }

        // GET: sitting/Details/5
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

        // GET: sitting/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: sitting/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryDescription")] Sitting category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: sitting/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sitting == null)
            {
                return NotFound();
            }

            var category = await _context.Sitting.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: sitting/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription")] Sitting category)
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

        // GET: sitting/Delete/5
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

        // POST: sitting/Delete/5
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
