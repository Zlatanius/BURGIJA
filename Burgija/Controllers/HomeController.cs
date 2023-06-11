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

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Burgija.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        private string[] categories = {
        "Fasteners",
        "FloorCare",
        "GeneralConstruction",
        "LaddersAndScaffolding",
        "LawnCare",
        "MaterialHandling",
        "PaintAndDryWall",
        "PlumbingAndPumps",
        "PortablePower",
        "PowerTools",
        "PressureWashers",
        "RotaryOrDemolition",
        "TileSaws",
        "Trailers",
        "TreeCare",
        "Trenchers",
        "Welding"
        };

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
            if (search == null)
                return View(await _context.ToolType.ToListAsync());
            ViewBag.Search = search;
            List<ToolType> searchResults = await _context.ToolType.Where(t => t.Name.ToLower().Contains(search.ToLower())).ToListAsync();
            List<ToolType> searchCategoryResults = await _context.ToolType.Where(p => p.Category == Category.Fasteners).ToListAsync();
            searchResults.AddRange(searchCategoryResults);

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
            /*var parameter = new SqlParameter("@id", toolType.Id);

            var query = "select l.address as Address, s.id as StoreId, count(t.id) as Quantity from location l, store s, tool t, tooltype tt where l.id = s.locationid and s.id = t.storeid and t.tooltypeid = tt.id and  tt.id = @id group by l.address,s.id";

            var results = await _context.ToolAndStore.FromSqlRaw(query,parameter).ToListAsync();*/


            var results = await _context.Tool
            .Join(_context.Store, t => t.StoreId, s => s.Id, (t, s) => new { Tool = t, Store = s })
            .Join(_context.Location, ts => ts.Store.LocationId, l => l.Id, (ts, l) => new { ts.Tool, ts.Store, Location = l })
            .Join(_context.ToolType, tsl => tsl.Tool.ToolTypeId, tt => tt.Id, (tsl, tt) => new { tsl.Tool, tsl.Store, tsl.Location, ToolType = tt })
            .Where(result => result.ToolType.Id == toolType.Id)
            .GroupBy(result => new { result.Location.Address, result.Store.Id })
            .Select(group => new ToolAndStore
            (
                group.Key.Address,
                group.Key.Id,
                group.Count()
            ))
            .ToListAsync();

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

















      



        public async Task<IEnumerable<ToolType>> GetToolTypesFromIds(List<int> ids)
        {
            var tooltypes = new List<ToolType>();
            foreach (var id in ids)
            {
                tooltypes.Add(await _context.ToolType.FirstAsync(t => t.Id == id));
            }
            return tooltypes;
        }

        
    }
}
