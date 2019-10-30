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
            if (fromWhere == "conference")
            {
                ViewData["ConferenceId"] = conferenceId;
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
                ViewData["From"] = "conference";
            }
            else
            {
                ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name");
                ViewData["RoomId"] = roomId;
                ViewData["From"] = "room";
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
                _context.Add(practicalSessionsEvent);
                await _context.SaveChangesAsync();
                if (fromWhere == "conference")
                {
                    return RedirectToAction("Details", "Conference", new { id = practicalSessionsEvent.ConferenceId });
                }
                return RedirectToAction("Details", "Room", new { id = practicalSessionsEvent.RoomId });
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
                    _context.Update(practicalSessionsEvent);
                    await _context.SaveChangesAsync();
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
                if (fromWhere == "Conference")
                {
                    return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.ConferenceId });
                }
                return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.RoomId });
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
            if (fromWhere == "Conference")
            {
                return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.ConferenceId });
            }
            return RedirectToAction("Details", fromWhere, new { id = practicalSessionsEvent.RoomId });
        }

        private bool PracticalSessionsEventExists(int id)
        {
            return _context.PracticalSessionsEvents.Any(e => e.Id == id);
        }
    }
}
