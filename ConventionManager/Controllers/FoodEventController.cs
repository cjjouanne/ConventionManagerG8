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
    public class FoodEventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FoodEventController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FoodEvent
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FoodEvents.Include(f => f.Conference).Include(f => f.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FoodEvent/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodEvent = await _context.FoodEvents
                .Include(f => f.Conference)
                .Include(f => f.Room)
                .Include(s => s.Subscriptions)
                .FirstOrDefaultAsync(m => m.Id == id);
            var userId = _userManager.GetUserId(HttpContext.User);
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.EventId == foodEvent.Id);

            var eventAndSubscription = new EventAndSubscription()
            {
                Event = foodEvent,
                Subscription = subscription
            };

            if (foodEvent == null)
            {
                return NotFound();
            }

            return View(eventAndSubscription);
        }

        // GET: FoodEvent/Create
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

        // POST: FoodEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fromWhere, [Bind("Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] FoodEvent foodEvent)
        {
            if (ModelState.IsValid)
            {
                // Checks if dates are out of range
                var conference = await _context.Conferences.FirstAsync(n => n.Id == foodEvent.ConferenceId);
                var events = _context.Events.Where(e => e.Id != foodEvent.Id).ToArray();
                if (!foodEvent.CheckDateTime(conference))
                {
                    TempData["DateOutOfRange"] = foodEvent.OutOfRangeMessage;
                }
                else if (!foodEvent.CheckCollisionWithEvent(events))
                {
                    TempData["EventCollision"] = foodEvent.CollisionWithEventMessage;
                }
                else
                {
                    _context.Add(foodEvent);
                    await _context.SaveChangesAsync();
                }
                // Checks where the request came from to redirect correctly
                switch (fromWhere)
                {
                    case "Conference":
                        return RedirectToAction("Details", fromWhere, new { id = foodEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = foodEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = foodEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", foodEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", foodEvent.RoomId);
            return View(foodEvent);
        }

        // GET: FoodEvent/Edit/5
        public async Task<IActionResult> Edit(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodEvent = await _context.FoodEvents.FindAsync(id);
            if (foodEvent == null)
            {
                return NotFound();
            }
            var conference = await _context.Conferences.FirstOrDefaultAsync(c => c.Id == foodEvent.ConferenceId);
            var eventCenterId = conference.EventCenterId;

            ViewData["ConferenceId"] = new SelectList(_context.Conferences.Where(a => a.EventCenterId == eventCenterId), "Id", "Name", foodEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(a => a.EventCenterId == eventCenterId), "Id", "Name", foodEvent.RoomId);
            ViewData["From"] = fromWhere;
            return View(foodEvent);
        }

        // POST: FoodEvent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string fromWhere, [Bind("Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] FoodEvent foodEvent)
        {
            if (id != foodEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Checks if dates are out of range
                    var conference = await _context.Conferences.FirstAsync(n => n.Id == foodEvent.ConferenceId);
                    var events = _context.Events.Where(e => e.Id != foodEvent.Id).ToArray();
                    if (!foodEvent.CheckDateTime(conference))
                    {
                        TempData["DateOutOfRange"] = foodEvent.OutOfRangeMessage;
                    }
                    else if (!foodEvent.CheckCollisionWithEvent(events))
                    {
                        TempData["EventCollision"] = foodEvent.CollisionWithEventMessage;
                    }
                    else
                    {
                        _context.Update(foodEvent);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodEventExists(foodEvent.Id))
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
                        return RedirectToAction("Details", fromWhere, new { id = foodEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = foodEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = foodEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", foodEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", foodEvent.RoomId);
            return View(foodEvent);
        }

        // GET: FoodEvent/Delete/5
        public async Task<IActionResult> Delete(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodEvent = await _context.FoodEvents
                .Include(f => f.Conference)
                .Include(f => f.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodEvent == null)
            {
                return NotFound();
            }
            ViewData["From"] = fromWhere;
            return View(foodEvent);
        }

        // POST: FoodEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string fromWhere)
        {
            var foodEvent = await _context.FoodEvents.FindAsync(id);
            IEnumerable<Subscription> subscriptions = _context.Subscriptions.Where(s => s.Event.Id == id).ToArray();
            if (subscriptions.Any())
            {
                TempData["CannotDeleteEvent"] = foodEvent.CannotDeleteEventMessage;
            }
            else
            {
                _context.FoodEvents.Remove(foodEvent);
                await _context.SaveChangesAsync();
            }

            // Checks where the request came from to redirect correctly
            switch (fromWhere)
            {
                case "Conference":
                    return RedirectToAction("Details", fromWhere, new { id = foodEvent.ConferenceId });
                case "Room":
                    return RedirectToAction("Details", fromWhere, new { id = foodEvent.RoomId });
                default:
                    return RedirectToAction("Details", "Conference", new { id = foodEvent.ConferenceId });
            }
        }

        private bool FoodEventExists(int id)
        {
            return _context.FoodEvents.Any(e => e.Id == id);
        }
    }
}
