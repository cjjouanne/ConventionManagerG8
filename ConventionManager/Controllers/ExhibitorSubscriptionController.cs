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
    public class ExhibitorSubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ExhibitorSubscriptionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> CreateSubscription(int eventId)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (!_signInManager.IsSignedIn(HttpContext.User))
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = _userManager.GetUserId(HttpContext.User);

            var exhibitorSubscription = new ExhibitorSubscription()
            {
                UserId = userId,
                EventId = @event.Id,
                ConferenceId = @event.ConferenceId
            };

            // checks if subscription collides with date of other suscribed event
            // if true, no colission
            var otherEvents = _context.Events
                .Include(s => s.Subscriptions)
                .Where(e => e.Id != @event.Id).ToArray();
            if (exhibitorSubscription.SubscriptionCollision(otherEvents, @event))
            {
                _context.Add(exhibitorSubscription);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["SubscriptionCollision"] = exhibitorSubscription.CollisionWithEventMessage;
            }
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        public async Task<IActionResult> DeleteSubscription(int eventId)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            var userId = _userManager.GetUserId(HttpContext.User);
            var exhibitorSubscription = await _context.ExhibitorSubscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.EventId == @event.Id);

            _context.ExhibitorSubscriptions.Remove(exhibitorSubscription);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        private bool ExhibitorSubscriptionExists(int id)
        {
            return _context.ExhibitorSubscriptions.Any(e => e.Id == id);
        }
    }
}
