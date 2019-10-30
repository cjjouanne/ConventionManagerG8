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
    public class ChatEventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChatEvent
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChatEvents.Include(c => c.Conference).Include(c => c.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChatEvent/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(chatEvent);
        }

        // GET: ChatEvent/Create
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

        // POST: ChatEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fromWhere, [Bind("ModeratorId,ExhibitorsId,Topic,Id,Name,StartDate,EndDate,ConferenceId,RoomId,AttendantsId")] ChatEvent chatEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatEvent);
                await _context.SaveChangesAsync();
                if (fromWhere == "conference")
                {
                    return RedirectToAction("Details", "Conference", new { id = chatEvent.ConferenceId });
                }
                return RedirectToAction("Details", "Room", new { id = chatEvent.RoomId });
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", chatEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", chatEvent.RoomId);
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
                    _context.Update(chatEvent);
                    await _context.SaveChangesAsync();
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
                if (fromWhere == "Conference")
                {
                    return RedirectToAction("Details", fromWhere, new { id = chatEvent.ConferenceId });
                }
                return RedirectToAction("Details", fromWhere, new { id = chatEvent.RoomId });
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
            _context.ChatEvents.Remove(chatEvent);
            await _context.SaveChangesAsync();

            if (fromWhere == "Conference")
            {
                return RedirectToAction("Details", fromWhere, new { id = chatEvent.ConferenceId });
            }
            return RedirectToAction("Details", fromWhere, new { id = chatEvent.RoomId });
        }

        private bool ChatEventExists(int id)
        {
            return _context.ChatEvents.Any(e => e.Id == id);
        }
    }
}
