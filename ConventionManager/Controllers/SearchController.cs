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
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GlobalSearch()
        {
            var searchResults = new SearchResults();
            return View(searchResults);
        }

        [HttpPost]
        public async Task<IActionResult> GlobalSearch(string searchString)
        {
            var eventCenters = _context.EventCenters.Where(ev => ev.Name.Contains(searchString)).ToArray();
            var conferences = _context.Conferences.Where(c => c.Name.Contains(searchString)).ToArray();
            var rooms = _context.Rooms.Where(r => r.Name.Contains(searchString)).ToArray();
            var users = _context.Users.Where(u =>
                                                u.FirstName.Contains(searchString) ||
                                                u.LastName.Contains(searchString) ||
                                                u.FullName().Contains(searchString) ||
                                                u.UserName.Contains(searchString)).ToList();

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            bool isInRole = await _userManager.IsInRoleAsync(user, "Organizer");
            IEnumerable<Event> events;
            if (isInRole)
            {
                events = _context.Events.Where(e => e.Name.Contains(searchString)).ToArray();
            }
            else
            {
                events = _context.Events.Where(e => e.Name.Contains(searchString) && DateTime.Compare(e.EndDate, DateTime.Now) > 0).ToArray();
            }

            var searchResults = new SearchResults();

            if (eventCenters.Any())
            {
                searchResults.EventCenters = eventCenters;
            }
            if (conferences.Any())
            {
                searchResults.Conferences = conferences;
            }
            if (events.Any())
            {
                searchResults.Events = events;
            }
            if (rooms.Any())
            {
                searchResults.Rooms = rooms;
            }
            if (users.Any())
            {
                searchResults.Users = users;
            }

            ViewData["searchInput"] = searchString;

            return View(searchResults);
        }
    }
}
