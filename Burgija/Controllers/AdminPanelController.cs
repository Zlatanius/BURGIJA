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
        private List<Store> stores;
        private List<ToolType> toolTypes;

        public AdminPanelController(ApplicationDbContext context)
        {
            _context = context;
            stores = _context.Store.ToList();
            foreach (Store store in stores)
            {
                store.StoreLocation =_context.Location.Find(store.LocationId);
            }
            toolTypes = _context.ToolType.ToList();
        }

        // GET: AdminPanel
        public async Task<IActionResult> Index()
        {
            var tools = await _context.Tools.ToListAsync();
            foreach(var tool in tools)
            {
                tool.Store = stores.Find(store => store.Id == tool.StoreId);
            }

            return View(tools);
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
            tool.Store = stores.Find(store => store.Id == tool.Store.Id);
            tool.ToolType = toolTypes.Find(toolType => toolType.Id == tool.ToolTypeId);
            return View(tool);
        }

        // GET: AdminPanel/AddTool
        public IActionResult AddTool()
        {
            ViewBag.Store = stores;
            ViewBag.ToolType = toolTypes;
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
            ViewBag.Store = stores;
            ViewBag.ToolType = toolTypes;
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
            tool.Store = stores.Find(store => store.Id == tool.Store.Id);
            tool.ToolType = toolTypes.Find(toolType => toolType.Id == tool.ToolTypeId);
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
