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
    public class EventCenterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventCenterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventCenter
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventCenters.ToListAsync());
        }

        // GET: EventCenter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCenter = await _context.EventCenters
                .Include(c => c.Conferences)
                .Include(c => c.Rooms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCenter == null)
            {
                return NotFound();
            }

            var eventCenterConferencesAndRooms = new EventCenterConferencesAndRooms();
            eventCenterConferencesAndRooms.EventCenter = eventCenter;

            return View(eventCenterConferencesAndRooms);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConference([FromRoute]int id, Conference conference)
        {
            var eventCenter = await _context.EventCenters
                .Include(c => c.Conferences)
                .FirstAsync(n => n.Id == id);

            eventCenter.Conferences.Add(conference);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromRoute]int id, Room room)
        {
            var eventCenter = await _context.EventCenters
                .Include(c => c.Rooms)
                .FirstAsync(n => n.Id == id);

            eventCenter.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }

        // GET: EventCenter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventCenter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Location")] EventCenter eventCenter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventCenter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventCenter);
        }

        // GET: EventCenter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCenter = await _context.EventCenters.FindAsync(id);
            if (eventCenter == null)
            {
                return NotFound();
            }
            return View(eventCenter);
        }

        // POST: EventCenter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Location")] EventCenter eventCenter)
        {
            if (id != eventCenter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventCenter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCenterExists(eventCenter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventCenter);
        }

        // GET: EventCenter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCenter = await _context.EventCenters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCenter == null)
            {
                return NotFound();
            }

            return View(eventCenter);
        }

        // POST: EventCenter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventCenter = await _context.EventCenters.FindAsync(id);
            _context.EventCenters.Remove(eventCenter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventCenterExists(int id)
        {
            return _context.EventCenters.Any(e => e.Id == id);
        }
    }
}
