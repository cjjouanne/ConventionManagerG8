using System;
namespace ConventionManager.Models
{
    public class EventAndSubscription
    {
        public Event Event { get; set; }

        public ExhibitorEvent ExhibitorEvent { get; set; }

        public Subscription Subscription { get; set; }

        public Notification Notification { get; set; }
    }
}
