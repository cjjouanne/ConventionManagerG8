using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConventionManager.Data;
using ConventionManager.Models;
using Microsoft.AspNetCore.Identity;

namespace ConventionManager.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Notification
        public async Task<IActionResult> Index()
        {
            var notifications = _context.Notifications.Include(n => n.User)
                                            .Where(n => n.UserId == _userManager.GetUserId(HttpContext.User));
            return View(await notifications.ToListAsync());
        }

        // GET: Notification/Details/5

        // GET: Notification/Create
        public IActionResult Create(int eventId)
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewBag.EventId = eventId;
            return View();
        }

        // POST: Notification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,SubscriptionId,Message,SentOn")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", notification.UserId);
            return View(notification);
        }

        // GET: Notification/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> SendNotificationToEventAttendants(int eventId, string message)
        {    
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            var subscriptions = _context.AttendantSubscriptions.Where(s => s.EventId == eventId).ToArray();
            foreach (var s in subscriptions)
            {
                var notification = new Notification() {
                    UserId = _userManager.GetUserId(HttpContext.User),
                    SubscriptionId = s.Id,
                    Message = message,
                    SentOn = DateTime.Now
                };
                _context.Add(notification);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
