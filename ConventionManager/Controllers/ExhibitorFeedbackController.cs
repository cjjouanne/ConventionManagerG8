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
    public class ExhibitorFeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExhibitorFeedbackController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExhibitorFeedback.Include(e => e.Exhibitor);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int eventId, [Bind("Id,UserId,ExhibitorId,Overall,Preparation,Attitude,Voice,Connection,Other,Date")] ExhibitorFeedback exhibitorFeedback)
        {
            var @event = _context.Events.FirstOrDefault(e => e.Id == eventId);
            var currentUserId = _userManager.GetUserId(HttpContext.User);
            var allFeedbackOnThisExhibitor = _context.ExhibitorFeedback.Where(ef => ef.ExhibitorId == exhibitorFeedback.ExhibitorId).ToList();

            foreach (var ef in allFeedbackOnThisExhibitor)
            {
                if (ef.UserId == currentUserId)
                {
                    TempData["FeedbackRequest"] = "Feedback not submitted, you have alerady sent us feedback on this exhibitor.";
                    return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
                }
            }
            TempData["FeedbackRequest"] = "Feedback successfully submitted!";

            if (ModelState.IsValid)
            {
                exhibitorFeedback.UserId = currentUserId;
                _context.Add(exhibitorFeedback);
                await _context.SaveChangesAsync();
            }
            ViewData["ExhibitorId"] = new SelectList(_context.Users, "Id", "Id", exhibitorFeedback.ExhibitorId);
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exhibitorFeedback = await _context.ExhibitorFeedback.FindAsync(id);
            _context.ExhibitorFeedback.Remove(exhibitorFeedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExhibitorFeedbackExists(int id)
        {
            return _context.ExhibitorFeedback.Any(e => e.Id == id);
        }
    }
}
