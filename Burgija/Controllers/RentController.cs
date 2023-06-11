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

namespace Burgija.Controllers
{
    public class RentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private ToolType toolType;

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

            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Id");
            ViewData["ToolId"] = new SelectList(_context.Tool, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewBag.ToolType = toolType;
            // Pass the tool type to the view
            return View();
        }

        // POST: Rent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ToolId,StartOfRent,EndOfRent,DiscountId")] Rent rent)
        {
            rent.UserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            rent.ToolId = 1;
            rent.DiscountId = null;
            if (ModelState.IsValid)
            {
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Id", rent.DiscountId);
            ViewData["ToolId"] = new SelectList(_context.Tool, "Id", "Id", rent.ToolId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rent.UserId);
            return View(rent);
        }

        // GET: Rent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Id", rent.DiscountId);
            ViewData["ToolId"] = new SelectList(_context.Tool, "Id", "Id", rent.ToolId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rent.UserId);
            return View(rent);
        }

        // POST: Rent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ToolId,StartOfRent,EndOfRent,DiscountId")] Rent rent)
        {
            if (id != rent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.Id))
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
            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Id", rent.DiscountId);
            ViewData["ToolId"] = new SelectList(_context.Tool, "Id", "Id", rent.ToolId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rent.UserId);
            return View(rent);
        }

        // GET: Rent/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Rent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rent.FindAsync(id);
            _context.Rent.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rent.Any(e => e.Id == id);
        }
    }
}
