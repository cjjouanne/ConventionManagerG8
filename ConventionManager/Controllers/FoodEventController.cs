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
    public class FoodEventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FoodEvent
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FoodEvents.Include(f => f.Conference).Include(f => f.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FoodEvent/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(foodEvent);
        }

        // GET: FoodEvent/Create
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
                var room = await _context.Rooms.FirstAsync(n => n.Id == foodEvent.RoomId);
                if (!foodEvent.CheckDateTime(conference))
                {
                    TempData["DateOutOfRange"] = foodEvent.OutOfRangeMessage;
                }
                else if (!foodEvent.CheckCollisionWithEvent(conference, room))
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Name", foodEvent.ConferenceId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", foodEvent.RoomId);
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
                    var room = await _context.Rooms.FirstAsync(n => n.Id == foodEvent.RoomId);
                    if (!foodEvent.CheckDateTime(conference))
                    {
                        TempData["DateOutOfRange"] = foodEvent.OutOfRangeMessage;
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
            _context.FoodEvents.Remove(foodEvent);
            await _context.SaveChangesAsync();
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
