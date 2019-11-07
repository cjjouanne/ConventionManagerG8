using System;
using System.Collections;
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
            var notifications = _context.Notifications.Where(n => n.UserId == _userManager.GetUserId(HttpContext.User));
            List<NotificationConferenceAndEvent> results = new List<NotificationConferenceAndEvent>();
            foreach (var notification in notifications)
            {
                var notificationConferenceAndEvent = new NotificationConferenceAndEvent();
                notificationConferenceAndEvent.Notification = notification;

                var subscription = _context.Subscriptions.First(s => s.Id == notification.SubscriptionId);

                notificationConferenceAndEvent.Conference = _context.Conferences
                                                            .First(c => c.Id == subscription.ConferenceId);

                notificationConferenceAndEvent.Event = _context.Events
                                                        .First(e => e.Id == subscription.EventId);

                var user = await _userManager.FindByIdAsync(notification.SentByUserId);
                notificationConferenceAndEvent.UserName = user.UserName;
                results.Add(notificationConferenceAndEvent);
            }
            return View(results);
        }
        // GET: Notification/Create
        public IActionResult Create(int id, string type, string receivers)
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Notification/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SendConferenceNotification(int id, string receivers, string message,
            bool mailing)
        {
            var conference = await _context.Conferences.FirstOrDefaultAsync(c => c.Id == id);
            IEnumerable<Subscription> subscriptions = new List<Subscription>();
            if (receivers == "Attendants")
            {
                subscriptions = _context.AttendantSubscriptions.Where(s => s.ConferenceId == id).ToArray();
            }
            else if (receivers == "Exhibitors")
            {
                subscriptions = _context.ExhibitorSubscriptions.Where(s => s.ConferenceId == id).ToArray();
            }
            foreach (Subscription s in subscriptions)
            {
                Notification notification = new Notification() {
                    SentByUserId = _userManager.GetUserId(HttpContext.User),
                    UserId = s.UserId,
                    SubscriptionId = s.Id,
                    ConferenceId = conference.Id,
                    Message = message,
                    SentOn = DateTime.Now,
                    Type = "Conference"
                };

                var alreadySent = _context.Notifications.FirstOrDefault(n => n.UserId == s.UserId && n.ConferenceId == conference.Id && n.Message == message);

                if (alreadySent == null)
                {
                    _context.Add(notification);
                    await _context.SaveChangesAsync();
                    if (mailing)
                    {
                        var user = _context.Users.First(u => u.Id == s.UserId);
                        var address = user.Email;
                        var subject = "Notification from Conference " + conference.Name;
                        SendMail(address, message, subject);
                    }
                }
            }
            return RedirectToAction("Details", "Conference", new { id = conference.Id });
        }
        public async Task<IActionResult> SendEventNotification(int id, string receivers, string message, bool mailing)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            IEnumerable<Subscription> subscriptions = new List<Subscription>();
            if (receivers == "Attendants")
            {
                subscriptions = _context.AttendantSubscriptions.Where(s => s.Event.Id == id).ToArray();
            }
            else if (receivers == "Exhibitors")
            {
                subscriptions = _context.ExhibitorSubscriptions.Where(s => s.Event.Id == id).ToArray();
            }
            foreach (var s in subscriptions)
            {
                var notification = new Notification() {
                    SentByUserId = _userManager.GetUserId(HttpContext.User),
                    UserId = s.UserId,
                    SubscriptionId = s.Id,
                    EventId = @event.Id,
                    Message = message,
                    SentOn = DateTime.Now,
                    Type = "Event"
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
