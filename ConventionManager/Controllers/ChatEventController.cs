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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ConventionManager.Controllers
{
    [Authorize(Roles = "Organizer")]
    public class ChatEventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatEventController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ChatEvent
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChatEvents.Include(c => c.Conference).Include(c => c.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChatEvent/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatEvent = await _context.ChatEvents
                .Include(c => c.Conference)
                .Include(c => c.Room)
                .Include(s => s.Subscriptions)
                .FirstOrDefaultAsync(m => m.Id == id);
            var userId = _userManager.GetUserId(HttpContext.User);
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.EventId == chatEvent.Id);
            
            var eventAndSubscription = new EventAndSubscription()
            {
                ExhibitorEvent = chatEvent,
                Subscription = subscription,
                ChatEvent = chatEvent,
                UserId = userId
            };
            
            if (chatEvent.ModeratorId != "0")
            {
                eventAndSubscription.Moderator = _context.Users.FirstOrDefault(u => u.Id == chatEvent.ModeratorId);
            }

            return View(eventAndSubscription);
        }

        // GET: ChatEvent/Create
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

        // POST: ChatEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fromWhere, [Bind("ModeratorId,ExhibitorsId,Topic,Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] ChatEvent chatEvent)
        {
            if (ModelState.IsValid)
            {
                // Checks if dates are out of range
                var conference = await _context.Conferences.FirstAsync(n => n.Id == chatEvent.ConferenceId);
                var events = _context.Events.Where(e => e.Id != chatEvent.Id).ToArray();
                if (!chatEvent.CheckDateTime(conference))
                {
                    TempData["DateOutOfRange"] = chatEvent.OutOfRangeMessage;
                }
                else if (!chatEvent.CheckCollisionWithEvent(events))
                {
                    TempData["EventCollision"] = chatEvent.CollisionWithEventMessage;
                }
                else
                {
                    _context.Add(chatEvent);
                    await _context.SaveChangesAsync();
                }

                // Checks where the request came from to redirect correctly
                switch (fromWhere)
                {
                    case "Conference":
                        return RedirectToAction("Details", fromWhere, new { id = chatEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = chatEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = chatEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", chatEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", chatEvent.RoomId);
            return View(chatEvent);
        }

        // GET: ChatEvent/Edit/5
        public async Task<IActionResult> Edit(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatEvent = await _context.ChatEvents.FindAsync(id);
            if (chatEvent == null)
            {
                return NotFound();
            }
            var conference = await _context.Conferences.FirstOrDefaultAsync(c => c.Id == chatEvent.ConferenceId);
            var eventCenterId = conference.EventCenterId;

            ViewData["ConferenceId"] = new SelectList(_context.Conferences.Where(a => a.EventCenterId == eventCenterId), "Id", "Name", chatEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(a => a.EventCenterId == eventCenterId), "Id", "Name", chatEvent.RoomId);
            ViewData["From"] = fromWhere;
            return View(chatEvent);
        }

        // POST: ChatEvent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string fromWhere, [Bind("ModeratorId,ExhibitorsId,Topic,Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] ChatEvent chatEvent)
        {
            if (id != chatEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Checks if dates are out of range
                    var conference = await _context.Conferences.FirstAsync(n => n.Id == chatEvent.ConferenceId);
                    var events = _context.Events.Where(e => e.Id != chatEvent.Id).ToArray();
                    if (!chatEvent.CheckDateTime(conference))
                    {
                        TempData["DateOutOfRange"] = chatEvent.OutOfRangeMessage;
                    }
                    else if (!chatEvent.CheckCollisionWithEvent(events))
                    {
                        TempData["EventCollision"] = chatEvent.CollisionWithEventMessage;
                    }
                    else
                    {
                        _context.Update(chatEvent);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatEventExists(chatEvent.Id))
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
                        return RedirectToAction("Details", fromWhere, new { id = chatEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = chatEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = chatEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", chatEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", chatEvent.RoomId);
            return View(chatEvent);
        }

        // GET: ChatEvent/Delete/5
        public async Task<IActionResult> Delete(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatEvent = await _context.ChatEvents
                .Include(c => c.Conference)
                .Include(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatEvent == null)
            {
                return NotFound();
            }
            ViewData["From"] = fromWhere;
            return View(chatEvent);
        }

        // POST: ChatEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string fromWhere)
        {
            var chatEvent = await _context.ChatEvents.FindAsync(id);
            IEnumerable<Subscription> subscriptions = _context.Subscriptions.Where(s => s.Event.Id == id).ToArray();
            if (subscriptions.Any())
            {
                TempData["CannotDeleteEvent"] = chatEvent.CannotDeleteEventMessage;
            }
            else
            {
                _context.ChatEvents.Remove(chatEvent);
                await _context.SaveChangesAsync();
            }

            // Checks where the request came from to redirect correctly
            switch (fromWhere)
            {
                case "Conference":
                    return RedirectToAction("Details", fromWhere, new { id = chatEvent.ConferenceId });
                case "Room":
                    return RedirectToAction("Details", fromWhere, new { id = chatEvent.RoomId });
                default:
                    return RedirectToAction("Details", "Conference", new { id = chatEvent.ConferenceId });
            }
        }

        public async Task<IActionResult> AddModerator(int id)
        {
            var chatEvent = _context.ChatEvents.First(e => e.Id == id);
            chatEvent.ModeratorId = _userManager.GetUserId(HttpContext.User);
            _context.Update(chatEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ChatEvent", new { id = chatEvent.Id });
        }
        
        public async Task<IActionResult> RemoveModerator(int id)
        {
            var chatEvent = _context.ChatEvents.First(e => e.Id == id);
            chatEvent.ModeratorId = "0";
            _context.Update(chatEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ChatEvent", new { id = chatEvent.Id });
        }

        private bool ChatEventExists(int id)
        {
            return _context.ChatEvents.Any(e => e.Id == id);
        }
    }
}
