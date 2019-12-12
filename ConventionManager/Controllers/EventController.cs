using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConventionManager.Data;
using ConventionManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ConventionManager.Controllers
{
    [Authorize(Roles = "Organizer")]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                bool isInRole = await _userManager.IsInRoleAsync(user, "Organizer");
                if (isInRole)
                {
                    return View(await _context.Events.OrderBy(c => c.StartDate).ToListAsync());
                }
            }
            return View(_context.Events.OrderBy(c => c.StartDate).Where(e => DateTime.Compare(e.EndDate, DateTime.Now) > 0));
        }

        [AllowAnonymous]
        public async Task<IActionResult> ViewEventsByTopic()
        {
            var exhibitorEventsAndTopics = new ExhibitorEventsAndTopics();

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            bool isInRole = await _userManager.IsInRoleAsync(user, "Organizer");

            if (isInRole)
            {
                exhibitorEventsAndTopics.ExhibitorEvents = _context.ExhibitorEvents.ToList();
            }
            else
            {
                exhibitorEventsAndTopics.ExhibitorEvents = _context.ExhibitorEvents.Where(e => DateTime.Compare(e.EndDate, DateTime.Now) > 0).ToList();
            }

            // All topics
            List<string> topics = new List<string>();
            foreach (ExhibitorEvent exhibitorEvent in _context.ExhibitorEvents)
            {
                topics.Add(exhibitorEvent.Topic);
            }
            var hashSet = new HashSet<string>(topics);
            List<string> filteredTopics = hashSet.ToList();
            exhibitorEventsAndTopics.Topics = filteredTopics;

            return View(exhibitorEventsAndTopics);
        }

        public async Task<IActionResult> ShowStatistics(int id)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            var feedbacks = _context.EventFeedback.Where(ef => ef.EventId == id).ToList();

            var statistics = @event.GetRating(feedbacks);

            return View(statistics);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
