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
    public class TalkEventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TalkEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TalkEvent
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TalkEvents.Include(t => t.Conference).Include(t => t.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TalkEvent/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(talkEvent);
        }

        // GET: TalkEvent/Create
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

        // POST: TalkEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fromWhere, [Bind("ExhibitorsId,Topic,Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] TalkEvent talkEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(talkEvent);
                await _context.SaveChangesAsync();
                if (fromWhere == "conference")
                {
                    return RedirectToAction("Details", "Conference", new { id = talkEvent.ConferenceId });
                }
                return RedirectToAction("Details", "Room", new { id = talkEvent.RoomId });
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", talkEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", talkEvent.RoomId);
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
                    _context.Update(talkEvent);
                    await _context.SaveChangesAsync();
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
                if (fromWhere == "Conference")
                {
                    return RedirectToAction("Details", fromWhere, new { id = talkEvent.ConferenceId });
                }
                return RedirectToAction("Details", fromWhere, new { id = talkEvent.RoomId });
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
            if (fromWhere == "Conference")
            {
                return RedirectToAction("Details", fromWhere, new { id = talkEvent.ConferenceId });
            }
            return RedirectToAction("Details", fromWhere, new { id = talkEvent.RoomId });
        }

        private bool TalkEventExists(int id)
        {
            return _context.TalkEvents.Any(e => e.Id == id);
        }
    }
}
