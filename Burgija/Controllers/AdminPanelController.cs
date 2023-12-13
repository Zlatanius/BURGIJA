using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Burgija.Data;
using Burgija.Models;
using Microsoft.AspNetCore.Authorization;

namespace Burgija.Controllers
{
    // Authorize only users with the "Administrator" role to access this controller
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController : Controller
    {
        // Database context for interacting with the underlying data store
        private readonly ApplicationDbContext _context;

        // Lists to store information about stores and tool types
        private List<Store> stores;
        private List<ToolType> toolTypes;

        // Constructor that takes the application database context as a dependency
        public AdminPanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Helper method to initialize lists of stores and tool types asynchronously.
        /// </summary>
        private async Task InitializeLists()
        {
            stores = await _context.Store.ToListAsync();
            List<Location> locations = await _context.Location.ToListAsync();
            foreach (Store store in stores)
            {
                store.StoreLocation = locations.Find(location => location.Id == store.LocationId);
            }
            toolTypes = await _context.ToolType.ToListAsync();
        }

        /// <summary>
        /// Displays a list of tools in the admin panel.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            await InitializeLists();
            var tools = await _context.Tool.ToListAsync();
            foreach (var tool in tools)
            {
                tool.Store = stores.Find(store => store.Id == tool.StoreId);
            }
            return View(tools);
        }

        /// <summary>
        /// Displays details of a specific tool in the admin panel.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tool = await _context.Tool
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

        /// <summary>
        /// Displays a form for adding a new tool in the admin panel.
        /// </summary>
        public async Task<IActionResult> AddTool()
        {
            await InitializeLists();
            ViewBag.Store = stores;
            ViewBag.ToolType = toolTypes;
            return View();
        }

        /// <summary>
        /// Handles the submission of the form for adding a new tool.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTool([Bind("Id,ToolTypeId,StoreId,Price,Image")] Tool tool)
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

        /// <summary>
        /// Displays a form for editing a specific tool in the admin panel.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tool = await _context.Tool.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }
            await InitializeLists();
            ViewBag.Store = stores;
            ViewBag.ToolType = toolTypes;
            return View(tool);
        }

        /// <summary>
        /// Handles the submission of the form for editing a specific tool.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ToolTypeId,StoreId,Price,Image")] Tool tool)
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

        /// <summary>
        /// Displays details of a specific tool for removal in the admin panel.
        /// </summary>
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tool = await _context.Tool
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

        /// <summary>
        /// Handles the confirmation of removing a specific tool.
        /// </summary>
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var tool = await _context.Tool.FindAsync(id);
            _context.Tool.Remove(tool);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a tool with the specified ID exists.
        /// </summary>
        private bool ToolExists(int id)
        {
            return _context.Tool.Any(e => e.Id == id);
        }
    }


}
