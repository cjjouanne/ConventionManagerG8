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
    public class TalkEventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TalkEventController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TalkEvent
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TalkEvents.Include(t => t.Conference).Include(t => t.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TalkEvent/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talkEvent = await _context.TalkEvents
                .Include(t => t.Conference)
                .Include(t => t.Room)
                .Include(s => s.Subscriptions)
                .FirstOrDefaultAsync(m => m.Id == id);
            var userId = _userManager.GetUserId(HttpContext.User);
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.EventId == talkEvent.Id);

            var eventAndSubscription = new EventAndSubscription()
            {
                ExhibitorEvent = talkEvent,
                Subscription = subscription
            };

            if (talkEvent == null)
            {
                return NotFound();
            }

            return View(eventAndSubscription);
        }

        // GET: TalkEvent/Create
        public IActionResult Create(int? conferenceId, int? roomId, string fromWhere, int eventCenterId)
        {
            if (fromWhere == "Conference")
            {
                ViewData["ConferenceId"] = conferenceId;
                ViewData["RoomId"] = new SelectList(_context.Rooms.Where(a => a.EventCenterId == eventCenterId), "Id", "Name");
                ViewData["From"] = "Conference";
            }
            else
            {
                ViewData["ConferenceId"] = new SelectList(_context.Conferences.Where(a => a.EventCenterId == eventCenterId), "Id", "Name");
                ViewData["RoomId"] = roomId;
                ViewData["From"] = "Room";
            }
            return View();
        }

        // POST: TalkEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fromWhere, [Bind("ExhibitorsId,Topic,Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] TalkEvent talkEvent)
        {
            if (ModelState.IsValid)
            {
                // Checks if dates are out of range
                var conference = await _context.Conferences.FirstAsync(n => n.Id == talkEvent.ConferenceId);
                var events = _context.Events.Where(e => e.Id != talkEvent.Id).ToArray();
                if (!talkEvent.CheckDateTime(conference))
                {
                    TempData["DateOutOfRange"] = talkEvent.OutOfRangeMessage;
                }
                else if (!talkEvent.CheckCollisionWithEvent(events))
                {
                    TempData["EventCollision"] = talkEvent.CollisionWithEventMessage;
                }
                else
                {
                    _context.Add(talkEvent);
                    await _context.SaveChangesAsync();
                }
                // Checks where the request came from to redirect correctly
                switch (fromWhere)
                {
                    case "Conference":
                        return RedirectToAction("Details", fromWhere, new { id = talkEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = talkEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = talkEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", talkEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", talkEvent.RoomId);
            return View(talkEvent);
        }

        // GET: TalkEvent/Edit/5
        public async Task<IActionResult> Edit(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talkEvent = await _context.TalkEvents.FindAsync(id);
            if (talkEvent == null)
            {
                return NotFound();
            }
            var conference = await _context.Conferences.FirstOrDefaultAsync(c => c.Id == talkEvent.ConferenceId);
            var eventCenterId = conference.EventCenterId;

            ViewData["ConferenceId"] = new SelectList(_context.Conferences.Where(a => a.EventCenterId == eventCenterId), "Id", "Name", talkEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(a => a.EventCenterId == eventCenterId), "Id", "Name", talkEvent.RoomId);
            ViewData["From"] = fromWhere;
            return View(talkEvent);
        }

        // POST: TalkEvent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string fromWhere, [Bind("ExhibitorsId,Topic,Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] TalkEvent talkEvent)
        {
            if (id != talkEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Checks if dates are out of range
                    var conference = await _context.Conferences.FirstAsync(n => n.Id == talkEvent.ConferenceId);
                    var events = _context.Events.Where(e => e.Id != talkEvent.Id).ToArray();
                    if (!talkEvent.CheckDateTime(conference))
                    {
                        TempData["DateOutOfRange"] = talkEvent.OutOfRangeMessage;
                    }
                    else if (!talkEvent.CheckCollisionWithEvent(events))
                    {
                        TempData["EventCollision"] = talkEvent.CollisionWithEventMessage;
                    }
                    else
                    {
                        _context.Update(talkEvent);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TalkEventExists(talkEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Checks where the request came from to redirect correctly
                switch (fromWhere)
                {
                    case "Conference":
                        return RedirectToAction("Details", fromWhere, new { id = talkEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = talkEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = talkEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", talkEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", talkEvent.RoomId);
            return View(talkEvent);
        }

        // GET: TalkEvent/Delete/5
        public async Task<IActionResult> Delete(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talkEvent = await _context.TalkEvents
                .Include(t => t.Conference)
                .Include(t => t.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (talkEvent == null)
            {
                return NotFound();
            }
            ViewData["From"] = fromWhere;
            return View(talkEvent);
        }

        // POST: TalkEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string fromWhere)
        {
            var talkEvent = await _context.TalkEvents.FindAsync(id);
            _context.TalkEvents.Remove(talkEvent);
            await _context.SaveChangesAsync();
            // Checks where the request came from to redirect correctly
            switch (fromWhere)
            {
                case "Conference":
                    return RedirectToAction("Details", fromWhere, new { id = talkEvent.ConferenceId });
                case "Room":
                    return RedirectToAction("Details", fromWhere, new { id = talkEvent.RoomId });
                default:
                    return RedirectToAction("Details", "Conference", new { id = talkEvent.ConferenceId });
            }
        }

        private bool TalkEventExists(int id)
        {
            return _context.TalkEvents.Any(e => e.Id == id);
        }
    }
}
