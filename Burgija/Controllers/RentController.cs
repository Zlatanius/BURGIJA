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

        public RentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="RegisteredUser")]
        public async Task<IActionResult> RentHistory()
        {
            // Extract the user ID from the claims associated with the current user
            var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Query the database to retrieve rent history information
            var rentHistory = await _context.Rent
                // Filter by the current user's ID
                .Where(r => r.UserId == userId)
                // Join the Rent table with the Tool table using the ToolId
                .Join(_context.Tool, rent => rent.ToolId, tool => tool.Id, (rent, tool) => new { Rent = rent, Tool = tool })
                // Join the result with the ToolType table using the ToolTypeId from the Tool table
                .Join(_context.ToolType, rt => rt.Tool.ToolTypeId, toolType => toolType.Id, (rt, toolType) => new RentAndToolType(rt.Rent, toolType))
                // Convert the result to a List asynchronously
                .ToListAsync();

            // Return the rent history to the corresponding view
            return View(rentHistory);
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
            // Check if the return date is earlier than the start date
            if (r.EndOfRent < r.StartOfRent)
            {
                return BadRequest("Date of return is earlier than date of taking");
            }

            // Check if the start date or return date is earlier than today
            if (r.StartOfRent < DateTime.Now || r.EndOfRent < DateTime.Now)
            {
                return BadRequest("Date of taking or date of return is earlier than today");
            }

            // Set the user ID from the claims associated with the current user
            r.UserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Retrieve the tool type ID from the session
            var toolTypeId = HttpContext.Session.GetInt32("ToolType");

            // Query the database to find available tools based on the tool type and date range
            var toolIds = await _context.Tool
                .Where(tool => tool.ToolType.Id == toolTypeId)
                .Where(tool => !_context.Rent.Any(rent =>
                    rent.ToolId == tool.Id &&
                    (r.EndOfRent < rent.StartOfRent || r.StartOfRent > rent.EndOfRent)))
                .Select(tool => tool.Id)
                .ToListAsync();

            // Check if available tools were found
            if (toolIds.Count > 0)
            {
                // Set the tool ID to the first available tool
                r.ToolId = toolIds[0];
            }
            else
            {
                // Return a BadRequest response if no tools are available in the specified period
                return BadRequest("There are no tools available in this period");
            }

            // To be implemented: Set the DiscountId (currently set to null as a placeholder)
            r.DiscountId = null;

            // Retrieve the tool type from the database
            var toolType = await _context.ToolType.FindAsync(toolTypeId);

            // Calculate the rent price based on the tool type's price and the rental duration
            r.RentPrice = toolType.Price * r.EndOfRent.Subtract(r.StartOfRent).TotalDays;

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Add the rent to the context and save changes
                _context.Add(r);
                await _context.SaveChangesAsync();

                // Redirect to the RentHistory action if successful
                return RedirectToAction("RentHistory");
            }

            // Redirect to the Create action with the tool type ID if the model state is not valid
            return RedirectToAction("Create", new { toolTypeId });
        }
    }
}
