using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Burgija.Data;
using Burgija.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using Microsoft.AspNetCore.Http;
using Burgija.ViewModels;

namespace Burgija.Controllers
{
    public class RentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public RentController(ApplicationDbContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rent
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rent.Include(r => r.Discount).Include(r => r.Tool).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> RentHistory()
        {
            var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rentHistory = await _context.Rent
            .Join(_context.Tool, rent => rent.ToolId, tool => tool.Id, (rent, tool) => new { Rent = rent, Tool = tool })
            .Join(_context.ToolType, rt => rt.Tool.ToolTypeId, toolType => toolType.Id, (rt, toolType) => new RentAndToolType(rt.Rent, toolType))
            .ToListAsync();
            return View(rentHistory);
        }

        // GET: Rent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent
                .Include(r => r.Discount)
                .Include(r => r.Tool)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }
        [HttpPost]
        public IActionResult GetToolType(int? toolTypeId)
        {
            if (toolTypeId == null)
            {
                return NotFound();
            }
            // Store the toolType value in session
            HttpContext.Session.SetInt32("ToolType", (int)toolTypeId);
            // Redirect to the Create action with the tool type ID
            return RedirectToAction("Create", new { toolTypeId });
        }

        // GET: Rent/Create
        public async Task<IActionResult> Create(int? toolTypeId)
        {
            if (toolTypeId == null)
            {
                return NotFound();
            }

            // Retrieve the tool type using the provided ID
            var toolType = await _context.ToolType.FirstOrDefaultAsync(m => m.Id == toolTypeId);

            if (toolType == null)
            {
                return NotFound();
            }

            ViewBag.ToolTypeImage = toolType.Image;

            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Id");
            ViewData["ToolId"] = new SelectList(_context.Tool, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewBag.ToolType = toolType;

            return View();
        }

        // POST: Rent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartOfRent,EndOfRent,DiscountId")] Rent r)
        {
            if(r.EndOfRent<r.StartOfRent)
                throw new Exception("Merso 1");
            if (r.StartOfRent<DateTime.Now || r.EndOfRent < DateTime.Now)
                throw new Exception("Merso 2");
            r.UserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var toolTypeId = HttpContext.Session.GetInt32("ToolType");
            var toolIds = await _context.Tool
            .Where(tool => tool.ToolType.Id == toolTypeId)
            .Where(tool => !_context.Rent.Any(rent =>
                rent.ToolId == tool.Id &&
                (r.EndOfRent < rent.StartOfRent || r.StartOfRent > rent.EndOfRent)))
            .Select(tool => tool.Id)
            .ToListAsync();
            if (toolIds.Count > 0)
                r.ToolId = toolIds[0];
            else
                throw new Exception("Merso 3");
            r.DiscountId = null;
            var toolType = await _context.ToolType.FindAsync(toolTypeId);
            r.RentPrice = toolType.Price * r.EndOfRent.Subtract(r.StartOfRent).TotalDays;
            if (ModelState.IsValid)
            {
                _context.Add(r);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Id", r.DiscountId);
            ViewData["ToolId"] = new SelectList(_context.Tool, "Id", "Id", r.ToolId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", r.UserId);
            return View(r);
        }

    }
}
