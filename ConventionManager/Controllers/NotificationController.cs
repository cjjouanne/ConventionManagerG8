using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConventionManager.Data;
using ConventionManager.Models;
using Microsoft.AspNetCore.Identity;

namespace ConventionManager.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Notification
        public async Task<IActionResult> Index()
        {
            var notifications = _context.Notifications.Include(n => n.User)
                .Where(n => n.UserId == _userManager.GetUserId(HttpContext.User));
            return View(await notifications.ToListAsync());
        }
        // GET: Notification/Create
        public IActionResult Create(int eventId)
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["EventId"] = eventId;
            return View();
        }

        // POST: Notification/Delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> SendNotificationToEventAttendants(int eventId, string message, bool mailing)
        {    
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            var subscriptions = _context.AttendantSubscriptions.Where(s => s.Event.Id == eventId).ToArray();
            foreach (var s in subscriptions)
            {
                var notification = new Notification() {
                    UserId = s.UserId,
                    SubscriptionId = s.Id,
                    Message = message,
                    SentOn = DateTime.Now
                };
                _context.Add(notification);
                await _context.SaveChangesAsync();
                if(mailing)
                {
                    var user = _context.Users.First(u => u.Id == s.UserId);
                    var address = user.Email;
                    var subject = "Notification from Event " + @event.Name;
                    SendMail(address, message, subject);
                }
            }
            return RedirectToAction("Details", @event.GetEventType(), new { id = @event.Id });
        }

        public void SendMail(string address, string message, string subject)
        {
            // Credentials
            var credentials = new NetworkCredential("apikey", "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");

            // Mail message
            var mail = new MailMessage
            {
                From = new MailAddress("notifications@conferences.com", "ConferenceManager"),
                Body = message,
                Subject = subject,
                IsBodyHtml = false,
            };

            mail.To.Add(address);

            // Smtp client
            var client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.sendgrid.com",
                EnableSsl = false,
                Credentials = credentials
            };

            client.Send(mail);
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
