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
            var events = _context.Events.Where(e => e.Name.Contains(searchString)).ToArray();
            var rooms = _context.Rooms.Where(r => r.Name.Contains(searchString)).ToArray();
            var user = await _userManager.FindByNameAsync(searchString);

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
            if (user != null)
            {
                searchResults.User = user;
            }

            ViewData["searchInput"] = searchString;

            return View(searchResults);
        }
    }
}
