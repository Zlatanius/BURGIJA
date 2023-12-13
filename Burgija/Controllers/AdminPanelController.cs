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
    // Controller for managing tools in the admin panel
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
        private async Task InitializeListsAsync()
        {
            // Retrieve the list of stores from the database
            stores = await _context.Store.ToListAsync();
            // Retrieve the list of locations from the database
            List<Location> locations = await _context.Location.ToListAsync();
            // Assign locations to the corresponding stores
            foreach (Store store in stores)
            {
                store.StoreLocation = locations.Find(location => location.Id == store.LocationId);
            }
            // Retrieve the list of tool types from the database
            toolTypes = await _context.ToolType.ToListAsync();
        }

        /// <summary>
        /// Displays a list of tools in the admin panel.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Initialize lists of stores and tool types
            await InitializeListsAsync();
            // Retrieve the list of tools from the database
            var tools = await _context.Tool.ToListAsync();
            // Assign stores to the corresponding tools
            foreach (var tool in tools)
            {
                tool.Store = stores.Find(store => store.Id == tool.StoreId);
            }
            // Return the view with the list of tools
            return View(tools);
        }

        /// <summary>
        /// Displays details of a specific tool in the admin panel.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the tool ID is null
            if (id == null)
            {
                return NotFound();
            }
            // Retrieve the tool details from the database, including store and tool type information
            var tool = await _context.Tool
                .Include(t => t.Store)
                .Include(t => t.ToolType)
                .FirstOrDefaultAsync(m => m.Id == id);
            // Check if the tool is not found
            if (tool == null)
            {
                return NotFound();
            }
            // Initialize lists of stores and tool types
            await InitializeListsAsync();
            // Assign store and tool type information to the tool
            tool.Store = stores.Find(store => store.Id == tool.Store.Id);
            tool.ToolType = toolTypes.Find(toolType => toolType.Id == tool.ToolTypeId);
            // Return the view with tool details
            return View(tool);
        }

        /// <summary>
        /// Displays a form for adding a new tool in the admin panel.
        /// </summary>
        public async Task<IActionResult> AddTool()
        {
            // Initialize lists of stores and tool types
            await InitializeListsAsync();
            // Set ViewBag properties for stores and tool types
            ViewBag.Store = stores;
            ViewBag.ToolType = toolTypes;
            // Return the view for adding a new tool
            return View();
        }

        /// <summary>
        /// Handles the submission of the form for adding a new tool.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTool([Bind("Id,ToolTypeId,StoreId,Price,Image")] Tool tool)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Add the new tool to the database
                _context.Add(tool);
                await _context.SaveChangesAsync();
                // Redirect to the index action after successful addition
                return RedirectToAction(nameof(Index));
            }
            // Set ViewData properties for stores and tool types
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id", tool.StoreId);
            ViewData["ToolTypeId"] = new SelectList(_context.ToolType, "Id", "Id", tool.ToolTypeId);
            // Return the view with the form for adding a new tool
            return View(tool);
        }

        /// <summary>
        /// Displays a form for editing a specific tool in the admin panel.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the tool ID is null
            if (id == null)
            {
                return NotFound();
            }
            // Retrieve the tool details from the database
            var tool = await _context.Tool.FindAsync(id);
            // Check if the tool is not found
            if (tool == null)
            {
                return NotFound();
            }
            // Initialize lists of stores and tool types
            await InitializeListsAsync();
            // Set ViewBag properties for stores and tool types
            ViewBag.Store = stores;
            ViewBag.ToolType = toolTypes;
            // Return the view with the form for editing a tool
            return View(tool);
        }

        /// <summary>
        /// Handles the submission of the form for editing a specific tool.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ToolTypeId,StoreId,Price,Image")] Tool tool)
        {
            // Check if the provided ID matches the tool ID
            if (id != tool.Id)
            {
                return NotFound();
            }

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the tool in the database
                    _context.Update(tool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
               {
                  {
           {
                // Check if the tool exists in the database
                if (!ToolExists(tool.Id))
                {
                    return NotFound();
                }
                else
                {
                    // Propagate the exception if there is a concurrency issue
                    throw;
                }

                // Redirect to the index action after successful edit
                return RedirectToAction(nameof(Index));
            }

            // Set ViewData properties for stores and tool types
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id", tool.StoreId);
            ViewData["ToolTypeId"] = new SelectList(_context.ToolType, "Id", "Id", tool.ToolTypeId);

            // Return the view with the form for editing a tool
            return View(tool);



            /// <summary>
            /// Displays details of a specific tool for removal in the admin panel.
            /// </summary>
            public async Task<IActionResult> Remove(int? id)
            {
                // Check if the tool ID is null
                if (id == null)
                {
                    return NotFound();
                }
                // Retrieve the tool details from the database, including store and tool type information
                var tool = await _context.Tool
                    .Include(t => t.Store)
                    .Include(t => t.ToolType)
                    .FirstOrDefaultAsync(m => m.Id == id);
                // Check if the tool is not found
                if (tool == null)
                {
                    return NotFound();
                }
                // Initialize lists of stores and tool types
                await InitializeListsAsync();
                // Assign store and tool type information to the tool
                tool.Store = stores.Find(store => store.Id == tool.Store.Id);
                tool.ToolType = toolTypes.Find(toolType => toolType.Id == tool.ToolTypeId);
                // Return the view with tool details for removal confirmation
                return View(tool);
            }

            /// <summary>
            /// Handles the confirmation of removing a specific tool.
            /// </summary>
            [HttpPost, ActionName("Remove")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> RemoveConfirmed(int id)
            {
                // Retrieve the tool from the database
                var tool = await _context.Tool.FindAsync(id);
                // Remove the tool from the database
                _context.Tool.Remove(tool);
                await _context.SaveChangesAsync();
                // Redirect to the index action after successful removal
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
