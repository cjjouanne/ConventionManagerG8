using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConventionManager.Data;
using ConventionManager.Models;
using Microsoft.AspNetCore.Identity;

namespace ConventionManager.Controllers
{
    public class EventFeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventFeedbackController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EventFeedback.Include(e => e.Event);
            return View(await applicationDbContext.ToListAsync());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,Overall,Organization,Attention,RoomQuality,Duration,WouldRecommend,Other,FoodQuality,Date")] EventFeedback eventFeedback)
        {
            var @event = _context.Events.FirstOrDefault(e => e.Id == eventFeedback.EventId);
            var currentUserId = _userManager.GetUserId(HttpContext.User);
            var allFeedbackOnThisEvent = _context.EventFeedback.Where(ef => ef.EventId == eventFeedback.EventId).ToList();

            foreach (var ef in allFeedbackOnThisEvent)
            {
                if (ef.UserId == currentUserId)
                {
                    TempData["FeedbackRequest"] = "Feedback not submitted, you have alerady sent us feedback on this event.";
                    return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
                }
            }
            TempData["FeedbackRequest"] = "Feedback successfully submitted!";

            if (ModelState.IsValid)
            {
                eventFeedback.UserId = currentUserId;
                _context.Add(eventFeedback);
                await _context.SaveChangesAsync();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator", eventFeedback.EventId);
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }
        
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
