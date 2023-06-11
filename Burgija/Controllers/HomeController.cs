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

            var reviews = await _context.Review
            .Join(
                _context.Tool,
                review => review.ToolId,
                tool => tool.Id,
                (review, tool) => new { Review = review, Tool = tool }
            )
            .Join(
                _context.Users,
                reviewAndTool => reviewAndTool.Review.UserId,
                user => user.Id,
                (reviewAndTool, user) => new { ReviewAndTool = reviewAndTool, User = user }
            )
            .Where(result => result.ReviewAndTool.Tool.ToolTypeId == id)
            .Select(result => new ReviewAndUser(
                result.ReviewAndTool.Review.RatingId,
                result.ReviewAndTool.Review.Text,
                result.User.UserName
            )
            )
            .ToListAsync();


            var toolAndStores = await _context.Tool
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

            ViewBag.ToolAndStore = toolAndStores;
            ViewBag.Review = reviews;
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
    }
}
