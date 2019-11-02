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
using System.Security.Claims;

namespace ConventionManager.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterToEvent([FromRoute]int eventId)
        {
            var @event = await _context.Events.FirstAsync(e => e.Id == eventId);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var room = await _context.Rooms.FirstAsync(r => r.Id == @event.RoomId);
            var conference = await _context.Conferences.FirstAsync(c => c.Id == @event.ConferenceId);

            if (room.Vacancies > 0)
            {
                room.Occupancy += 1;
                @event.AttendantsId.Add(userId);
                _context.Update(@event);
                await _context.SaveChangesAsync();
                await RegisterToConference(conference);
            }
            else
            {
                TempData["NoVacancies"] = "There are no more vacancies in this room for this Event";
            }
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        public async Task<IActionResult> UnRegisterFromEvent(Event @event)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var room = await _context.Rooms.FirstAsync(r => r.Id == @event.RoomId);
            var conference = await _context.Conferences.FirstAsync(c => c.Id == @event.ConferenceId);

            if (room.Vacancies < room.Capacity)
            {
                room.Occupancy -= 1;
                @event.AttendantsId.Remove(userId);
                await UnRegisterFormConference(conference);
                _context.Update(room);
                _context.Update(@event);
                await _context.SaveChangesAsync();
            }            
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        public async Task<IActionResult> RegisterToConference(Conference conference)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!conference.AttendantsId.Contains(userId))
            {
                conference.AttendantsId.Add(userId);
                _context.Update(conference);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Conference", new { id = conference.Id });
        }

        public async Task<IActionResult> UnRegisterFormConference(Conference conference)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (conference.AttendantsId.Contains(userId))
            {
                conference.AttendantsId.Remove(userId);
                _context.Update(conference);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Conference", new { id = conference.Id });
        }
    }
}
