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
    public class SittingTableController : Controller
    {
        private readonly ReservationAppDbContext _context;

        public SittingTableController(ReservationAppDbContext context)
        {
            _context = context;
        }

        // GET: sittingTable
        public async Task<IActionResult> Index()
        {
              return View(await _context.SittingTable.ToListAsync());
        }

        // GET: sittingTable/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SittingTable == null)
            {
                return NotFound();
            }

            var sittingTable = await _context.SittingTable
                .FirstOrDefaultAsync(m => m.SittingTableId == id);
            if (sittingTable == null)
            {
                return NotFound();
            }

            return View(sittingTable);
        }

        // GET: sittingTable/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: sittingTable/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SittingTableID,Area,Table,Capacity")] SittingTable sittingTable)
        //public async Task<IActionResult> Create(SittingTable sittingTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sittingTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sittingTable);
        }

        // GET: sittingTable/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SittingTable == null)
            {
                return NotFound();
            }

            var sittingTable = await _context.SittingTable.FindAsync(id);
            if (sittingTable == null)
            {
                return NotFound();
            }
            return View(sittingTable);
        }

        // POST: sittingTable/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SittingTableId,Area,Table,Capacity")] SittingTable sittingTable)
        {
            if (id != sittingTable.SittingTableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sittingTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!sittingTableExists(sittingTable.SittingTableId))
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
            return View(sittingTable);
        }

        // GET: sittingTable/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SittingTable == null)
            {
                return NotFound();
            }

            var sittingTable = await _context.SittingTable
                .FirstOrDefaultAsync(m => m.SittingTableId == id);
            if (sittingTable == null)
            {
                return NotFound();
            }

            return View(sittingTable);
        }

        // POST: sittingTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SittingTable == null)
            {
                return Problem("Entity set 'ReservationAppDbContext.SittingTable'  is null.");
            }
            var sittingTable = await _context.SittingTable.FindAsync(id);
            if (sittingTable != null)
            {
                _context.SittingTable.Remove(sittingTable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool sittingTableExists(int id)
        {
          return _context.SittingTable.Any(e => e.SittingTableId == id);
        }
    }
}
