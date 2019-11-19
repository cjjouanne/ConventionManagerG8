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
    public class ConferenceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConferenceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Conference
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
                    return View(await _context.Conferences.OrderBy(c => c.StartDate).ToListAsync());
                }
            }
            return View(_context.Conferences.OrderBy(c => c.StartDate).Where(c => DateTime.Compare(c.EndDate, DateTime.Now) > 0));
        }

        // GET: Conference/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences
                .Include(c => c.Sponsors)
                .Include(e => e.Events)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conference == null)
            {
                return NotFound();
            }

            var conferenceAndSponsor = new ConferenceAndSponsor();
            conferenceAndSponsor.Conference = conference;


            return View(conferenceAndSponsor);
        }

        [HttpPost]
        public async Task<IActionResult> AddSponsor([FromRoute]int id, Sponsor sponsor)
        {
            var conference = await _context.Conferences
                .Include(c => c.Sponsors)
                .FirstAsync(n => n.Id == id);

            conference.Sponsors.Add(sponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> ChooseEventCenter()
        {
            return View(await _context.EventCenters.ToListAsync());
        }

        public async Task<IActionResult> ChooseEventType([FromRoute] int id)
        {
            var conference = await _context.Conferences.FirstAsync(n => n.Id == id);

            var conferenceEventAndRoom = new ConferenceEventAndRoom();
            conferenceEventAndRoom.Conference = conference;

            return View(conferenceEventAndRoom);
        }

        [HttpGet]
        public RedirectToActionResult PreCreateEvent(int conferenceId, string controllerName)
        {
            var conference = _context.Conferences.FirstOrDefault(c => c.Id == conferenceId);
            var eventCenter = _context.EventCenters.FirstOrDefault(e => e.Id == conference.EventCenterId);
            return RedirectToAction("Create", controllerName, new
            {
                conferenceId,
                fromWhere = "Conference",
                eventCenterId = eventCenter.Id
            });
        }

        // GET: Conference/Create
        public async Task<IActionResult> Create([FromRoute]int id)
        {
            var eventCenter = await _context.EventCenters.FirstAsync(n => n.Id == id);

            var eventCenterConferencesAndRooms = new EventCenterConferencesAndRooms();
            eventCenterConferencesAndRooms.EventCenter = eventCenter;
            return View(eventCenterConferencesAndRooms);
        }

        // POST: Conference/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conference);
        }

        // GET: Conference/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences.FindAsync(id);
            if (conference == null)
            {
                return NotFound();
            }
            return View(conference);
        }

        // POST: Conference/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int eventCenterId, [Bind("Id,Name,Description,StartDate,EndDate")] Conference conference)
        {
            if (id != conference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conference);
                    _context.Entry(conference).Property(u => u.EventCenterId).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConferenceExists(conference.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "EventCenter", new { id = eventCenterId });
            }
            return View(conference);
        }

        // GET: Conference/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conference == null)
            {
                return NotFound();
            }

            return View(conference);
        }

        // POST: Conference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int eventCenterId)
        {
            var conference = await _context.Conferences.FindAsync(id);
            _context.Conferences.Remove(conference);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "EventCenter", new { id = eventCenterId });
        }

        private bool ConferenceExists(int id)
        {
            return _context.Conferences.Any(e => e.Id == id);
        }
    }
}
