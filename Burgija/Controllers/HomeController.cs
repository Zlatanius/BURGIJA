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
            await InitializeLists();
            var tools = await _context.Tools.ToListAsync();
            foreach (var tool in tools)
            {
                tool.Store = stores.Find(store => store.Id == tool.StoreId);
            }

            return View(tools);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools
                .Include(t => t.Store)
                .Include(t => t.ToolType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tool == null)
            {
                return NotFound();
            }
            await InitializeLists();
            tool.Store = stores.Find(store => store.Id == tool.Store.Id);
            tool.ToolType = toolTypes.Find(toolType => toolType.Id == tool.ToolTypeId);
            return View(tool);
        }

        private bool ToolExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
}
