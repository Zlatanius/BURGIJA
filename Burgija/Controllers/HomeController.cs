﻿using System;
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
using Burgija.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

        private readonly UserManager<IdentityUser<int>> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Home
        public async Task<IActionResult> Index(string search, double? priceFrom, double? priceTo, string sortOptions)
        {
            if (search == null && priceFrom == null && priceTo == null && sortOptions == null)
            {
                return View(await _context.ToolType.ToListAsync());
            }

            var query = _context.ToolType.AsQueryable();

            if (search != null)
            {
                query = query.Where(t => t.Name.ToLower().Contains(search.ToLower()));
            }

            if (priceFrom != null && priceTo != null)
            {
                query = query.Where(t => t.Price >= priceFrom && t.Price <= priceTo);
            }

            if (!string.IsNullOrEmpty(sortOptions))
            {
                switch (sortOptions)
                {
                    case "lowestPrice":
                        query = query.OrderBy(t => t.Price);
                        break;
                    case "highestPrice":
                        query = query.OrderByDescending(t => t.Price);
                        break;
                    case "alphabetical":
                        query = query.OrderBy(t => t.Name);
                        break;
                }
            }

            var filterResults = await query.ToListAsync();

            return View(filterResults);
        }


        public async Task<IActionResult> FilterTools(double? priceFrom, double? priceTo, string sortOptions)
        {
            List<ToolType>? filterResults;
            priceFrom ??= 0;
            priceTo ??= 10000;
            switch(sortOptions) {
                case "lowestPrice":
                    filterResults = await _context.ToolType.Where(t => t.Price>=priceFrom).Where(t=>t.Price<=priceTo).OrderBy(t=>t.Price).ToListAsync();
                    break;
                case "highestPrice":
                    filterResults = await _context.ToolType.Where(t => t.Price >= priceFrom).Where(t => t.Price <= priceTo).OrderByDescending(t => t.Price).ToListAsync();
                    break;
                case "alphabetical":
                    filterResults = await _context.ToolType.Where(t => t.Price >= priceFrom).Where(t => t.Price <= priceTo).OrderBy(t => t.Name).ToListAsync();
                    break;
                default:
                    filterResults = null; 
                    break;
            }
            var queryParameters = new Dictionary<string, string>
            {
                { "priceFrom", priceFrom.ToString() },
                { "priceTo", priceTo.ToString() },
                { "sortOptions", sortOptions }
            };
            return RedirectToAction("Index",queryParameters);
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

            var reviewsAndUsers = await _context.Review
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
                result.ReviewAndTool.Review.Rating,
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
            ViewBag.ReviewAndUser = reviewsAndUsers;
            ViewBag.NumberOfReviews = reviewsAndUsers.Count();

            if (reviewsAndUsers.Count != 0)
                ViewBag.AverageRating = reviewsAndUsers.Average<ReviewAndUser>(reviewAndUser => reviewAndUser.Rating);
            else ViewBag.AverageRating = 0;
            if (User.IsInRole("RegisteredUser"))
            {
                var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                    ViewBag.Username = user.UserName;
            }
            return View(toolType);
        }

        public async Task<IActionResult> WhereYouCanFIndUs()
        {
            List<Store> stores = await _context.Store.ToListAsync();
            List<Location> locations = await _context.Location.ToListAsync();
            ViewBag.Store = stores;
            ViewBag.Location = locations;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReview([Bind("Id")] Review review, int toolTypeId, string textbox, double rating)
        {
            var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rents = await _context.Rent
            .Join(_context.Users, r => r.UserId, u => u.Id, (rent, user) => new { Rent = rent, User = user })
            .Join(_context.Tool, ru => ru.Rent.ToolId, t => t.Id, (ru, tool) => new { ru.Rent, ru.User, Tool = tool })
            .Join(_context.ToolType, rt => rt.Tool.ToolTypeId, tt => tt.Id, (rt, toolType) => new { rt.Rent, rt.User, rt.Tool, ToolType = toolType })
            .Where(result => result.ToolType.Id == toolTypeId && result.User.Id == userId)
            .Select(result => result.Rent).ToListAsync();
            if (rents.Count == 0)
            {
                return BadRequest("You have not rented this tool before!");
            }
            review.UserId = userId;
            review.ToolId = rents[0].ToolId;
            review.RentId = rents[0].Id;
            review.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            review.Text = textbox;
            review.Rating = rating;
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("ToolDetails", new { id = toolTypeId });
            }
            else
            {
                return RedirectToAction("ToolDetails", new { id = toolTypeId });
            }
        }
    }
}
