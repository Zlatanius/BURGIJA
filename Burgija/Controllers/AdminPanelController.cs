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
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminPanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminPanel
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tools.Include(t => t.Store).Include(t => t.ToolType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AdminPanel/Details/5
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

            return View(tool);
        }

        // GET: AdminPanel/AddTool
        public IActionResult AddTool()
        {
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id");
            ViewData["ToolTypeId"] = new SelectList(_context.ToolType, "Id", "Id");
            return View();
        }

        

        // POST: AdminPanel/AddTool
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTool([Bind("Id,ToolTypeId,StoreId,Price")] Tool tool)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id", tool.StoreId);
            ViewData["ToolTypeId"] = new SelectList(_context.ToolType, "Id", "Id", tool.ToolTypeId);
            return View(tool);
        }

        // GET: AdminPanel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id", tool.StoreId);
            ViewData["ToolTypeId"] = new SelectList(_context.ToolType, "Id", "Id", tool.ToolTypeId);
            return View(tool);
        }

        // POST: AdminPanel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ToolTypeId,StoreId,Price")] Tool tool)
        {
            if (id != tool.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolExists(tool.Id))
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
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id", tool.StoreId);
            ViewData["ToolTypeId"] = new SelectList(_context.ToolType, "Id", "Id", tool.ToolTypeId);
            return View(tool);
        }

        // GET: AdminPanel/Delete/5
        public async Task<IActionResult> Remove(int? id)
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

            return View(tool);
        }

        // POST: AdminPanel/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var tool = await _context.Tools.FindAsync(id);
            _context.Tools.Remove(tool);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
}
