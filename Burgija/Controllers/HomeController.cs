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
    // HomeController class is responsible for handling requests related to the home page and tool-related functionalities.
    public class HomeController : Controller
    {
        // Application database context for interacting with the underlying data store.
        private readonly ApplicationDbContext _context;

        // Pre-defined categories of tools.
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

        // User manager for managing user-related operations.
        private readonly UserManager<IdentityUser<int>> _userManager;

        // Constructor that initializes the controller with the required dependencies.
        public HomeController(ApplicationDbContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays the home page with a list of tool types based on search, price range, and sorting options.
        /// </summary>
        public async Task<IActionResult> Index(string search, double? priceFrom, double? priceTo, string sortOptions)
        {
            // Check if no filters are applied, return all tool types.
            if (search == null && priceFrom == null && priceTo == null && sortOptions == null)
            {
                return View(await _context.ToolType.ToListAsync());
            }
            
            List<ToolType> filterResults = await _context.ToolType.ToListAsync();

            // Search by tool type name.
            if (search != null)
            {
                filterResults = LinearSearch(filterResults, search);
            }

            // Filter by price range.
            if (priceFrom != null && priceTo != null)
            {
                filterResults = LinearSearchByPrice(filterResults, (double)priceFrom, (double)priceTo);
            }

            // Sort results based on the selected sorting option.
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

        /// <summary>
        /// Handles filtering of tools based on price range and sorting options.
        /// </summary>
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

        /// <summary>
        /// Performs a case-insensitive linear search for tool types containing the specified search term.
        /// </summary>
        /// <param name="toolTypes">The list of tool types to search.</param>
        /// <param name="search">The search term.</param>
        /// <returns>A list of tool types matching the search term.</returns>
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

        /// <summary>
        /// Performs a linear search for tool types within the specified price range.
        /// </summary>
        /// <param name="toolTypes">The list of tool types to filter.</param>
        /// <param name="priceFrom">The minimum price.</param>
        /// <param name="priceTo">The maximum price.</param>
        /// <returns>A list of tool types within the specified price range.</returns>
        private static List<ToolType> LinearSearchByPrice(List<ToolType> toolTypes, double priceFrom, double priceTo)
        {
            List<ToolType> results = new List<ToolType>();

            foreach (var toolType in toolTypes)
            {
                if (toolType.Price >= priceFrom && toolType.Price <= priceTo)
                {
                    results.Add(toolType);
                }
            }

            return results;
        }

        /// <summary>
        /// Sorts a list of tool types using the Merge Sort algorithm in alphabetical order.
        /// </summary>
        /// <param name="list">The list of tool types to be sorted.</param>
        /// <returns>The sorted list of tool types.</returns>
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

        /// <summary>
        /// Merges two sorted lists of tool types into a single sorted list.
        /// </summary>
        /// <param name="left">The left sorted list.</param>
        /// <param name="right">The right sorted list.</param>
        /// <returns>The merged sorted list of tool types.</returns>
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

        /// <summary>
        /// Sorts a list of tool types using the QuickSort algorithm.
        /// </summary>
        /// <param name="toolTypes">The list of tool types to be sorted.</param>
        private static void QuickSort(List<ToolType> toolTypes)
        {
            QuickSort(toolTypes, 0, toolTypes.Count - 1);
        }

        /// <summary>
        /// Recursive helper method for the QuickSort algorithm.
        /// </summary>
        /// <param name="toolTypes">The list of tool types to be sorted.</param>
        /// <param name="low">The low index of the range to be sorted.</param>
        /// <param name="high">The high index of the range to be sorted.</param>
        private static void QuickSort(List<ToolType> toolTypes, int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(toolTypes, low, high);

                QuickSort(toolTypes, low, partitionIndex - 1);
                QuickSort(toolTypes, partitionIndex + 1, high);
            }
        }

        /// <summary>
        /// Partitions the list of tool types for the QuickSort algorithm.
        /// </summary>
        /// <param name="toolTypes">The list of tool types to be partitioned.</param>
        /// <param name="low">The low index of the range to be partitioned.</param>
        /// <param name="high">The high index of the range to be partitioned.</param>
        /// <returns>The partition index.</returns>
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

        /// <summary>
        /// Swaps two elements in a list of tool types.
        /// </summary>
        /// <param name="toolTypes">The list of tool types.</param>
        /// <param name="i">The index of the first element.</param>
        /// <param name="j">The index of the second element.</param>
        private static void Swap(List<ToolType> toolTypes, int i, int j)
        {
            ToolType temp = toolTypes[i];
            toolTypes[i] = toolTypes[j];
            toolTypes[j] = temp;
        }

        /// <summary>
        /// Sorts a list of tool types in descending order using the Selection Sort algorithm.
        /// </summary>
        /// <param name="toolTypes">The list of tool types to be sorted.</param>
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
                    Swap(toolTypes, i, maxIndex);
                }
            }
        }



        /// <summary>
        /// Displays details of a specific tool type, including reviews and store information.
        /// </summary>
        /// <param name="id">The ID of the tool type to display details for.</param>
        /// <returns>Returns a view containing details of the specified tool type.</returns>
        public async Task<IActionResult> ToolDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the tool type with the specified ID
            var toolType = await _context.ToolType.FirstOrDefaultAsync(m => m.Id == id);

            if (toolType == null)
            {
                return NotFound();
            }

            // Retrieve reviews and users associated with the tool type
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
                ))
                .ToListAsync();

            // Retrieve tool and store information associated with the tool type
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

            // Set ViewBag properties for the view
            ViewBag.ToolAndStore = toolAndStores;
            ViewBag.ReviewAndUser = reviewsAndUsers;
            ViewBag.NumberOfReviews = reviewsAndUsers.Count();

            if (reviewsAndUsers.Count != 0)
                ViewBag.AverageRating = reviewsAndUsers.Average<ReviewAndUser>(reviewAndUser => reviewAndUser.Rating);
            else
                ViewBag.AverageRating = 0;

            // Set the username if the user is in the "RegisteredUser" role
            if (User.IsInRole("RegisteredUser"))
            {
                var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                    ViewBag.Username = user.UserName;
            }

            // Return the view with details of the specified tool type
            return View(toolType);
        }


        /// <summary>
        /// Displays a view containing information about store locations.
        /// </summary>
        /// <returns>Returns a view containing information about store locations.</returns>
        public async Task<IActionResult> WhereYouCanFindUs()
        {
            // Retrieve the list of stores and locations from the database
            List<Store> stores = await _context.Store.ToListAsync();
            List<Location> locations = await _context.Location.ToListAsync();

            // Set ViewBag properties for the view
            ViewBag.Store = stores;
            ViewBag.Location = locations;

            // Return the view with information about store locations
            return View();
        }


        /// <summary>
        /// Processes and stores a user review for a specific tool type.
        /// </summary>
        /// <param name="review">The <see cref="Review"/> object containing review details.</param>
        /// <param name="toolTypeId">The ID of the tool type for which the review is submitted.</param>
        /// <param name="textbox">The text content of the review.</param>
        /// <param name="rating">The numerical rating assigned by the user.</param>
        /// <returns>
        /// - If the review is successfully added, redirects to the "ToolDetails" view for the specified tool type.
        /// - If there is an issue with the review submission, redirects to the "ToolDetails" view with an error message.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReview([Bind("Id")] Review review, int toolTypeId, string textbox, double rating)
        {
            // Retrieve the user ID from the claim
            var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Retrieve rents associated with the user and the specified tool type
            var rents = await _context.Rent
                .Join(_context.Users, r => r.UserId, u => u.Id, (rent, user) => new { Rent = rent, User = user })
                .Join(_context.Tool, ru => ru.Rent.ToolId, t => t.Id, (ru, tool) => new { ru.Rent, ru.User, Tool = tool })
                .Join(_context.ToolType, rt => rt.Tool.ToolTypeId, tt => tt.Id, (rt, toolType) => new { rt.Rent, rt.User, rt.Tool, ToolType = toolType })
                .Where(result => result.ToolType.Id == toolTypeId && result.User.Id == userId)
                .Select(result => result.Rent)
                .ToListAsync();

            // Check if the user has rented the specified tool type
            if (rents.Count == 0)
            {
                return BadRequest("You have not rented this tool before!");
            }

            // Set review details
            review.UserId = userId;
            review.ToolId = rents[0].ToolId;
            review.RentId = rents[0].Id;
            review.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            review.Text = textbox;
            review.Rating = rating;

            // Validate the review model
            if (ModelState.IsValid)
            {
                // Add the review to the database
                _context.Add(review);
                await _context.SaveChangesAsync();

                // Redirect to the "ToolDetails" view for the specified tool type
                return RedirectToAction("ToolDetails", new { id = toolTypeId });
            }
            else
            {
                // Redirect to the "ToolDetails" view for the specified tool type with an error message
                return RedirectToAction("ToolDetails", new { id = toolTypeId });
            }
        }
    }
}
