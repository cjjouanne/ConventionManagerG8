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
    public class ItineraryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItineraryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ShowItinerary()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _context.Users
                .Include(s => s.Subscriptions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            List<Event> eventsList = new List<Event>();

            bool isInRole = await _userManager.IsInRoleAsync(user, "Organizer");

            foreach (Subscription subscription in user.Subscriptions)
            {
                Event @event;
                if (isInRole)
                {
                    @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == subscription.EventId);
                    eventsList.Add(@event);
                }
                else
                {
                    @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == subscription.EventId && DateTime.Compare(e.EndDate, DateTime.Now) > 0);
                    eventsList.Add(@event);
                }
            }

            var allModeratorEvents = _context.ChatEvents.Where(ce => ce.ModeratorId == userId);
            foreach (Event moderatorEvent in allModeratorEvents)
            {
                if (isInRole)
                {
                    eventsList.Add(moderatorEvent);
                }
                else
                {
                    if (DateTime.Compare(moderatorEvent.EndDate, DateTime.Now) > 0)
                    {
                        eventsList.Add(moderatorEvent);
                    }
                }
            }

            return View(eventsList.OrderBy(c => c.StartDate));
        }
    }
}
