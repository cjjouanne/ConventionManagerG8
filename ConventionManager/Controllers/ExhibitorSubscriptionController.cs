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
    public class ExhibitorSubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExhibitorSubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExhibitorSubscription
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExhibitorSubscriptions.Include(e => e.Event);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExhibitorSubscription/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibitorSubscription = await _context.ExhibitorSubscriptions
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exhibitorSubscription == null)
            {
                return NotFound();
            }

            return View(exhibitorSubscription);
        }

        // GET: ExhibitorSubscription/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator");
            return View();
        }

        // POST: ExhibitorSubscription/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ConferenceId,EventId")] ExhibitorSubscription exhibitorSubscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exhibitorSubscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator", exhibitorSubscription.EventId);
            return View(exhibitorSubscription);
        }

        // GET: ExhibitorSubscription/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibitorSubscription = await _context.ExhibitorSubscriptions.FindAsync(id);
            if (exhibitorSubscription == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator", exhibitorSubscription.EventId);
            return View(exhibitorSubscription);
        }

        // POST: ExhibitorSubscription/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ConferenceId,EventId")] ExhibitorSubscription exhibitorSubscription)
        {
            if (id != exhibitorSubscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exhibitorSubscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExhibitorSubscriptionExists(exhibitorSubscription.Id))
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
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Discriminator", exhibitorSubscription.EventId);
            return View(exhibitorSubscription);
        }

        // GET: ExhibitorSubscription/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibitorSubscription = await _context.ExhibitorSubscriptions
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exhibitorSubscription == null)
            {
                return NotFound();
            }

            return View(exhibitorSubscription);
        }

        // POST: ExhibitorSubscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exhibitorSubscription = await _context.ExhibitorSubscriptions.FindAsync(id);
            _context.ExhibitorSubscriptions.Remove(exhibitorSubscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExhibitorSubscriptionExists(int id)
        {
            return _context.ExhibitorSubscriptions.Any(e => e.Id == id);
        }
    }
}
