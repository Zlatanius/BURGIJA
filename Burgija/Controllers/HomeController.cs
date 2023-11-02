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
using Burgija.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static Humanizer.On;

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

            if (search != null)
            {
                var toolTypes = await _context.ToolType.ToListAsync();
                var searchResults = LinearSearch(toolTypes, search);
                return View(searchResults);
            }

            List<ToolType> filterResults = await _context.ToolType.ToListAsync();

            if (priceFrom != null && priceTo != null)
            {
                filterResults = LinearSearchByPrice(filterResults, (double)priceFrom, (double)priceTo);
            }

            if (!string.IsNullOrEmpty(sortOptions))
            {
                switch (sortOptions)
                {
                    case "lowestPrice":
                        QuickSort(filterResults);
                        break;
                    case "highestPrice":
                        SelectionSortDescending(filterResults);
                        break;
                    case "alphabetical":
                        filterResults = MergeSort(filterResults);
                        break;
                }
            }

            return View(filterResults);
        }


        public Task<IActionResult> FilterTools(double? priceFrom, double? priceTo, string sortOptions)
        {
            priceFrom ??= 0;
            priceTo ??= 10000;
            var queryParameters = new Dictionary<string, string>
            {
                { "priceFrom", priceFrom.ToString() },
                { "priceTo", priceTo.ToString() },
                { "sortOptions", sortOptions }
            };
            return Task.FromResult<IActionResult>(RedirectToAction("Index", queryParameters));
        }

        private static List<ToolType> LinearSearch(List<ToolType> toolTypes, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return new List<ToolType>();
            }

            search = search.ToLower(); // Convert search to lowercase for case-insensitive search

            List<ToolType> results = new List<ToolType>();

            foreach (var toolType in toolTypes)
            {
                if (toolType.Name.ToLower().Contains(search))
                {
                    results.Add(toolType);
                }
            }

            return results;
        }
        private static List<ToolType> LinearSearchByPrice(List<ToolType> toolTypes, double priceFrom, double priceTo)
        { 
            List<ToolType> results = new List<ToolType>();

            foreach (var toolType in toolTypes)
            {
                if (toolType.Price>=priceFrom && toolType.Price<=priceTo)
                {
                    results.Add(toolType);
                }
            }

            return results;
        }

        private static List<ToolType> MergeSort(List<ToolType> list)
        {
            if (list.Count <= 1)
                return list;

            int middle = list.Count / 2;
            List<ToolType> left = list.GetRange(0, middle);
            List<ToolType> right = list.GetRange(middle, list.Count - middle);

            left = MergeSort(left);
            right = MergeSort(right);

            return Merge(left, right);
        }

        private static List<ToolType> Merge(List<ToolType> left, List<ToolType> right)
        {
            List<ToolType> result = new List<ToolType>();
            int leftIndex = 0;
            int rightIndex = 0;

            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                if (string.Compare(left[leftIndex].Name, right[rightIndex].Name, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    result.Add(left[leftIndex]);
                    leftIndex++;
                }
                else
                {
                    result.Add(right[rightIndex]);
                    rightIndex++;
                }
            }

            result.AddRange(left.GetRange(leftIndex, left.Count - leftIndex));
            result.AddRange(right.GetRange(rightIndex, right.Count - rightIndex));

            return result;
        }

        private static void QuickSort(List<ToolType> toolTypes)
        {
            QuickSort(toolTypes, 0, toolTypes.Count - 1);
        }

        private static void QuickSort(List<ToolType> toolTypes, int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(toolTypes, low, high);

                QuickSort(toolTypes, low, partitionIndex - 1);
                QuickSort(toolTypes, partitionIndex + 1, high);
            }
        }

        private static int Partition(List<ToolType> toolTypes, int low, int high)
        {
            double pivot = toolTypes[high].Price;
            int i = (low - 1);

            for (int j = low; j < high; j++)
            {
                if (toolTypes[j].Price < pivot)
                {
                    i++;
                    Swap(toolTypes, i, j);
                }
            }

            Swap(toolTypes, i + 1, high);
            return i + 1;
        }

        private static void Swap(List<ToolType> toolTypes, int i, int j)
        {
            ToolType temp = toolTypes[i];
            toolTypes[i] = toolTypes[j];
            toolTypes[j] = temp;
        }

        private static void SelectionSortDescending(List<ToolType> toolTypes)
        {
            int n = toolTypes.Count;

            for (int i = 0; i < n - 1; i++)
            {
                int maxIndex = i;

                for (int j = i + 1; j < n; j++)
                {
                    if (toolTypes[j].Price > toolTypes[maxIndex].Price)
                    {
                        maxIndex = j;
                    }
                }

                if (maxIndex != i)
                {
                    // Swap toolTypes[i] and toolTypes[maxIndex]
                    ToolType temp = toolTypes[i];
                    toolTypes[i] = toolTypes[maxIndex];
                    toolTypes[maxIndex] = temp;
                }
            }
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
