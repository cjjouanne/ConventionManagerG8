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

namespace ConventionManager.Controllers
{
    [Authorize(Roles = "Organizer")]
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Room
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.OrderBy(c => c.Id).ToListAsync());
        }

        // GET: Room/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(c => c.Events)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            var conferenceEventAndRoom = new ConferenceEventAndRoom();
            conferenceEventAndRoom.Room = room;

            return View(conferenceEventAndRoom);
        }

        public async Task<IActionResult> ChooseEventType([FromRoute] int id)
        {
            var room = await _context.Rooms.FirstAsync(n => n.Id == id);

            var conferenceEventAndRoom = new ConferenceEventAndRoom();
            conferenceEventAndRoom.Room = room;

            return View(conferenceEventAndRoom);
        }

        [HttpGet]
        public RedirectToActionResult PreCreateEvent(int roomId, string controllerName)
        {
            var room = _context.Rooms.FirstOrDefault(c => c.Id == roomId);
            var eventCenter = _context.EventCenters.FirstOrDefault(e => e.Id == room.EventCenterId);
            return RedirectToAction("Create", controllerName, new
            {
                roomId,
                fromWhere = "Room",
                eventCenterId = eventCenter.Id
            });
        }

        // GET: Room/Create
        public async Task<IActionResult> Create([FromRoute]int id)
        {
            var eventCenter = await _context.EventCenters.FirstAsync(n => n.Id == id);

            var eventCenterConferencesAndRooms = new EventCenterConferencesAndRooms();
            eventCenterConferencesAndRooms.EventCenter = eventCenter;
            return View(eventCenterConferencesAndRooms);
        }

        // POST: Room/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventCenterId,Name,Capacity,Location,Latitude,Longitude")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Room/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(ec => ec.EventCenter)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int eventCenterId, [Bind("Id,EventCenterId,Name,Capacity,Location,Latitude,Longitude")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    _context.Entry(room).Property(u => u.EventCenterId).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Useful for searching Entities in _context
                // var eventCenter = _context.EventCenters.Where(e => e.Id == room.EventCenter.Id).Single();
                // var eventCenter = _context.EventCenters.Find(room.EventCenterId);
                return RedirectToAction("Details", "EventCenter", new { id = eventCenterId });
            }
            return View(room);
        }

        // GET: Room/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int eventCenterId)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "EventCenter", new { id = eventCenterId });
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
