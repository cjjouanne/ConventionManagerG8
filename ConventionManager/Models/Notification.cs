using System;
using System.ComponentModel.DataAnnotations;

namespace ConventionManager.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int SubscriptionId { get; set; }

        [Required(ErrorMessage = "The message cannot be empty.")]
        public string Message { get; set; }

        public DateTime SentOn { get; set; }

        public string Type { get; set; }

        public int ConferenceId { get; set; }

        public int EventId { get; set; }
    }
}