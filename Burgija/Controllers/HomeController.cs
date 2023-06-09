using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Burgija.Data;
using Burgija.Models;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Burgija.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<Store> stores;
        private List<ToolType> toolTypes;
        private List<Review> reviews;
        private List<Tool> tools;
        private List<Location> locations;
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
        public async Task<IActionResult> Index(string search)
        {
            if(search == null)
                return View(await _context.ToolType.ToListAsync());
            ViewBag.Search = search;
            List<ToolType> searchResults = await _context.ToolType.Where(t => t.Name.ToLower().Contains(search.ToLower())).ToListAsync();
            return View(searchResults);
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
            var parameter = new SqlParameter("@id", toolType.Id);

            var query = "select l.address as Address, s.id as StoreId, count(t.id) as Quantity from location l, store s, tool t, tooltype tt where l.id = s.locationid and s.id = t.storeid and t.tooltypeid = tt.id and  tt.id = @id group by l.address,s.id";

            var results = await _context.ToolAndStore.FromSqlRaw(query,parameter).ToListAsync();

            foreach (var result in results)
            {
                var address = result.Address;
                var storeId = result.StoreId;
                var quantity = result.Quantity;
            }
            ViewBag.ToolAndStore = results;
            return View(toolType);
        }

        // GET: Home/Create
        public async Task<IActionResult> WhereYouCanFIndUs()
        {
            stores = await _context.Store.ToListAsync();
            locations = await _context.Location.ToListAsync();
            ViewBag.Store = stores;
            ViewBag.Location = locations;
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
