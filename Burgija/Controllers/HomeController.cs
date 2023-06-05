using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Burgija.Data;
using Burgija.Models;

namespace Burgija.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<Store> stores;
        private List<ToolType> toolTypes;
        private List<Review> reviews;
        private List<Tool> tools;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task InitializeLists()
        {
            stores = await _context.Store.ToListAsync();
            List<Location> locations = await _context.Location.ToListAsync();
            foreach (Store store in stores)
            {
                store.StoreLocation = locations.Find(location => location.Id == store.LocationId);
            }
            toolTypes = await _context.ToolType.ToListAsync();
            reviews = await _context.Review.ToListAsync();
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            return View(await _context.ToolType.ToListAsync());
        }

        // GET: Home/Details/5
        public async Task<IActionResult> ToolDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toolType = await _context.ToolType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toolType == null)
            {
                return NotFound();
            }
            

            return View(toolType);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Description,Price,Image")] ToolType toolType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toolType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toolType);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toolType = await _context.ToolType.FindAsync(id);
            if (toolType == null)
            {
                return NotFound();
            }
            return View(toolType);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Description,Price,Image")] ToolType toolType)
        {
            if (id != toolType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toolType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolTypeExists(toolType.Id))
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
            return View(toolType);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toolType = await _context.ToolType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toolType == null)
            {
                return NotFound();
            }

            return View(toolType);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toolType = await _context.ToolType.FindAsync(id);
            _context.ToolType.Remove(toolType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolTypeExists(int id)
        {
            return _context.ToolType.Any(e => e.Id == id);
        }
    }
}
