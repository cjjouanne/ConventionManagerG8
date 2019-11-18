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
    public class EventFeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventFeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventFeedback
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EventFeedback.Include(e => e.Event);
            return View(await applicationDbContext.ToListAsync());
        }
        
        // POST: EventFeedback/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,Overall,Organization,Attention,RoomQuality,Duration,WouldRecommend,Other,FoodQuality,Date")] EventFeedback eventFeedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventFeedback);
                await _context.SaveChangesAsync();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator", eventFeedback.EventId);
            var @event = _context.Events.First(e => e.Id == eventFeedback.EventId);
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }
        
        // POST: EventFeedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventFeedback = await _context.EventFeedback.FindAsync(id);
            _context.EventFeedback.Remove(eventFeedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventFeedbackExists(int id)
        {
            return _context.EventFeedback.Any(e => e.Id == id);
        }
    }
}
