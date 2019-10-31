using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConventionManager.Data;
using ConventionManager.Models;

namespace ConventionManager.Controllers
{
    public class PracticalSessionsEventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PracticalSessionsEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PracticalSessionsEvent
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PracticalSessionsEvents.Include(p => p.Conference).Include(p => p.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PracticalSessionsEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practicalSessionsEvent = await _context.PracticalSessionsEvents
                .Include(p => p.Conference)
                .Include(p => p.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practicalSessionsEvent == null)
            {
                return NotFound();
            }

            return View(practicalSessionsEvent);
        }

        // GET: PracticalSessionsEvent/Create
        public IActionResult Create(int? conferenceId, int? roomId, string fromWhere)
        {
            if (fromWhere == "Conference")
            {
                ViewData["ConferenceId"] = conferenceId;
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
                ViewData["From"] = "Conference";
            }
            else
            {
                ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name");
                ViewData["RoomId"] = roomId;
                ViewData["From"] = "Room";
            }
            return View();
        }

        // POST: PracticalSessionsEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fromWhere, [Bind("ExhibitorsId,Topic,Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] PracticalSessionsEvent practicalSessionsEvent)
        {
            if (ModelState.IsValid)
            {
                // Checks if dates are out of range
                var conference = await _context.Conferences.FirstAsync(n => n.Id == practicalSessionsEvent.ConferenceId);
                var events = _context.Events.Where(e => e.Id != practicalSessionsEvent.Id).ToArray();
                if (!practicalSessionsEvent.CheckDateTime(conference))
                {
                    TempData["DateOutOfRange"] = practicalSessionsEvent.OutOfRangeMessage;
                }
                else if (!practicalSessionsEvent.CheckCollisionWithEvent(events))
                {
                    TempData["EventCollision"] = practicalSessionsEvent.CollisionWithEventMessage;
                }
                else
                {
                    _context.Add(practicalSessionsEvent);
                    await _context.SaveChangesAsync();
                }
                // Checks where the request came from to redirect correctly
                switch (fromWhere)
                {
                    case "Conference":
                        return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = practicalSessionsEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", practicalSessionsEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", practicalSessionsEvent.RoomId);
            return View(practicalSessionsEvent);
        }

        // GET: PracticalSessionsEvent/Edit/5
        public async Task<IActionResult> Edit(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practicalSessionsEvent = await _context.PracticalSessionsEvents.FindAsync(id);
            if (practicalSessionsEvent == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", practicalSessionsEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", practicalSessionsEvent.RoomId);
            ViewData["From"] = fromWhere;
            return View(practicalSessionsEvent);
        }

        // POST: PracticalSessionsEvent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string fromWhere, [Bind("ExhibitorsId,Topic,Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] PracticalSessionsEvent practicalSessionsEvent)
        {
            if (id != practicalSessionsEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Checks if dates are out of range
                    var conference = await _context.Conferences.FirstAsync(n => n.Id == practicalSessionsEvent.ConferenceId);
                    var events = _context.Events.Where(e => e.Id != practicalSessionsEvent.Id).ToArray();
                    if (!practicalSessionsEvent.CheckDateTime(conference))
                    {
                        TempData["DateOutOfRange"] = practicalSessionsEvent.OutOfRangeMessage;
                    }
                    else if (!practicalSessionsEvent.CheckCollisionWithEvent(events))
                    {
                        TempData["EventCollision"] = practicalSessionsEvent.CollisionWithEventMessage;
                    }
                    else
                    {
                        _context.Update(practicalSessionsEvent);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracticalSessionsEventExists(practicalSessionsEvent.Id))
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
                        return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = practicalSessionsEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", practicalSessionsEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", practicalSessionsEvent.RoomId);
            return View(practicalSessionsEvent);
        }

        // GET: PracticalSessionsEvent/Delete/5
        public async Task<IActionResult> Delete(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practicalSessionsEvent = await _context.PracticalSessionsEvents
                .Include(p => p.Conference)
                .Include(p => p.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practicalSessionsEvent == null)
            {
                return NotFound();
            }
            ViewData["From"] = fromWhere;
            return View(practicalSessionsEvent);
        }

        // POST: PracticalSessionsEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string fromWhere)
        {
            var practicalSessionsEvent = await _context.PracticalSessionsEvents.FindAsync(id);
            _context.PracticalSessionsEvents.Remove(practicalSessionsEvent);
            await _context.SaveChangesAsync();
            // Checks where the request came from to redirect correctly
            switch (fromWhere)
            {
                case "Conference":
                    return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.ConferenceId });
                case "Room":
                    return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.RoomId });
                default:
                    return RedirectToAction("Details", "Conference", new { id = practicalSessionsEvent.ConferenceId });
            }
        }

        private bool PracticalSessionsEventExists(int id)
        {
            return _context.PracticalSessionsEvents.Any(e => e.Id == id);
        }
    }
}
