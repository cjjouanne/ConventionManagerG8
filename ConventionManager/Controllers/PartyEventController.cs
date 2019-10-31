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
    public class PartyEventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartyEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PartyEvent
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PartyEvents.Include(p => p.Conference).Include(p => p.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PartyEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partyEvent = await _context.PartyEvents
                .Include(p => p.Conference)
                .Include(p => p.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partyEvent == null)
            {
                return NotFound();
            }

            return View(partyEvent);
        }

        // GET: PartyEvent/Create
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

        // POST: PartyEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fromWhere, [Bind("Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] PartyEvent partyEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partyEvent);
                await _context.SaveChangesAsync();
                switch (fromWhere)
                {
                    case "Conference":
                        return RedirectToAction("Details", fromWhere, new { id = partyEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = partyEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = partyEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", partyEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", partyEvent.RoomId);
            return View(partyEvent);
        }

        // GET: PartyEvent/Edit/5
        public async Task<IActionResult> Edit(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partyEvent = await _context.PartyEvents.FindAsync(id);
            if (partyEvent == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", partyEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", partyEvent.RoomId);
            ViewData["From"] = fromWhere;
            return View(partyEvent);
        }

        // POST: PartyEvent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string fromWhere, [Bind("Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] PartyEvent partyEvent)
        {
            if (id != partyEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partyEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyEventExists(partyEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                switch (fromWhere)
                {
                    case "Conference":
                        return RedirectToAction("Details", fromWhere, new { id = partyEvent.ConferenceId });
                    case "Room":
                        return RedirectToAction("Details", fromWhere, new { id = partyEvent.RoomId });
                    default:
                        return RedirectToAction("Details", "Conference", new { id = partyEvent.ConferenceId });
                }
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", partyEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", partyEvent.RoomId);
            return View(partyEvent);
        }

        // GET: PartyEvent/Delete/5
        public async Task<IActionResult> Delete(int? id, string fromWhere)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partyEvent = await _context.PartyEvents
                .Include(p => p.Conference)
                .Include(p => p.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partyEvent == null)
            {
                return NotFound();
            }
            ViewData["From"] = fromWhere;
            return View(partyEvent);
        }

        // POST: PartyEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string fromWhere)
        {
            var partyEvent = await _context.PartyEvents.FindAsync(id);
            _context.PartyEvents.Remove(partyEvent);
            await _context.SaveChangesAsync();
            switch (fromWhere)
            {
                case "Conference":
                    return RedirectToAction("Details", fromWhere, new { id = partyEvent.ConferenceId });
                case "Room":
                    return RedirectToAction("Details", fromWhere, new { id = partyEvent.RoomId });
                default:
                    return RedirectToAction("Details", "Conference", new { id = partyEvent.ConferenceId });
            }
        }

        private bool PartyEventExists(int id)
        {
            return _context.PartyEvents.Any(e => e.Id == id);
        }
    }
}
