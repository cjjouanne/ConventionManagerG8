using System;

namespace ConventionManager.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int SubscriptionId { get; set; }
        public string Message { get; set; }
        public DateTime SentOn { get; set; } = DateTime.Now;
    }
}