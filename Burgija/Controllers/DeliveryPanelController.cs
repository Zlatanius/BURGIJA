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
using System.Security.Claims;

namespace Burgija.Controllers
{
    [Authorize(Roles = "Courier")]
    public class DeliveryPanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int LoggedInCourier;

        public DeliveryPanelController(ApplicationDbContext context)
        {
            _context = context;
            LoggedInCourier = 0; // dobiti trenutno ulogovanog usera 
        }

        // GET: DeliveryPanel
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Delivery.Where(delivery => delivery.CourierId == null);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> SeeAcceptedDeliveries()
        {
            var applicationDbContext = _context.Delivery.Where(delivery => delivery.CourierId == null);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DeliveryPanel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Delivery
                .FirstOrDefaultAsync(m => m.Id == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // GET: DeliveryPanel/MarkAsDelivered/5
        public async Task<IActionResult> MarkAsDelivered(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Delivery
                .FirstOrDefaultAsync(m => m.Id == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // POST: DeliveryPanel/Delete/5
        [HttpPost, ActionName("MarkAsDelivered")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsDeliveredConfirmed(int id)
        {
            var delivery = await _context.Delivery.FindAsync(id);
            _context.Delivery.Remove(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("MarkAsDelivered")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            var delivery = await _context.Delivery.FindAsync(id);
            delivery.CourierId = LoggedInCourier;
            _context.Delivery.Update(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryExists(int id)
        {
            return _context.Delivery.Any(e => e.Id == id);
        }
    }
}
