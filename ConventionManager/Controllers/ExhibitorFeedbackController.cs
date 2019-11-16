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
    public class ExhibitorFeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExhibitorFeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExhibitorFeedback
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExhibitorFeedback.Include(e => e.Exhibitor);
            return View(await applicationDbContext.ToListAsync());
        }

        // POST: ExhibitorFeedback/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int eventId, [Bind("Id,UserId,ExhibitorId,Overall,Preparation,Attitude,Voice,Connection,Other,Date")] ExhibitorFeedback exhibitorFeedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exhibitorFeedback);
                await _context.SaveChangesAsync();
            }
            ViewData["ExhibitorId"] = new SelectList(_context.Users, "Id", "Id", exhibitorFeedback.ExhibitorId);
            var @event = _context.Events.First(e => e.Id == eventId);
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        // POST: ExhibitorFeedback/Delete/5
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
