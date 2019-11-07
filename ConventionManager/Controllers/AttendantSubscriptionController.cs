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
    public class AttendantSubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendantSubscriptionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> CreateSubscription(int eventId)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            var userId = _userManager.GetUserId(HttpContext.User);

            var attendantSubscription = new AttendantSubscription() {
                UserId = userId,
                EventId = @event.Id,
                ConferenceId = @event.ConferenceId
            };

            // checks if subscription collides with date of other suscribed event
            // if true, no colission
            var otherEvents = _context.Events
                .Include(s => s.Subscriptions)
                .Where(e => e.Id != @event.Id).ToArray();
            if (attendantSubscription.SubscriptionCollision(otherEvents, @event))
            {
                _context.Add(attendantSubscription);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["SubscriptionCollision"] = attendantSubscription.CollisionWithEventMessage;
            }
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        public async Task<IActionResult> DeleteSubscription(int eventId)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            var userId = _userManager.GetUserId(HttpContext.User);
            var attendantSubscription = await _context.AttendantSubscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.EventId == @event.Id);

            _context.AttendantSubscriptions.Remove(attendantSubscription);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        private bool AttendantSubscriptionExists(int id)
        {
            return _context.AttendantSubscriptions.Any(e => e.Id == id);
        }
    }
}