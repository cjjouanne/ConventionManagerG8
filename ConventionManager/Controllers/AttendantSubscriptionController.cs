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
    public class AttendantSubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendantSubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AttendantSubscription
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AttendantSubscriptions.Include(a => a.Event);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AttendantSubscription/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendantSubscription = await _context.AttendantSubscriptions
                .Include(a => a.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendantSubscription == null)
            {
                return NotFound();
            }

            return View(attendantSubscription);
        }

        // GET: AttendantSubscription/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator");
            return View();
        }

        // POST: AttendantSubscription/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ConferenceId,EventId")] AttendantSubscription attendantSubscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendantSubscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator", attendantSubscription.EventId);
            return View(attendantSubscription);
        }

        // GET: AttendantSubscription/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendantSubscription = await _context.AttendantSubscriptions.FindAsync(id);
            if (attendantSubscription == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator", attendantSubscription.EventId);
            return View(attendantSubscription);
        }

        // POST: AttendantSubscription/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ConferenceId,EventId")] AttendantSubscription attendantSubscription)
        {
            if (id != attendantSubscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendantSubscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendantSubscriptionExists(attendantSubscription.Id))
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
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator", attendantSubscription.EventId);
            return View(attendantSubscription);
        }

        // GET: AttendantSubscription/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendantSubscription = await _context.AttendantSubscriptions
                .Include(a => a.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendantSubscription == null)
            {
                return NotFound();
            }

            return View(attendantSubscription);
        }

        // POST: AttendantSubscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendantSubscription = await _context.AttendantSubscriptions.FindAsync(id);
            _context.AttendantSubscriptions.Remove(attendantSubscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendantSubscriptionExists(int id)
        {
            return _context.AttendantSubscriptions.Any(e => e.Id == id);
        }
    }
}
