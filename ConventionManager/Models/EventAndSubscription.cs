using System;
namespace ConventionManager.Models
{
    public class EventAndSubscription
    {
        public Event Event { get; set; }

        public ExhibitorEvent ExhibitorEvent { get; set; }

        public Subscription Subscription { get; set; }

        public Notification Notification { get; set; }

        public ChatEvent ChatEvent { get; set; }

        public FoodEvent FoodEvent { get; set; }

        public string UserId { get; set; }

        public ApplicationUser Moderator { get; set; }
    }
}
