using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public abstract class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ConferenceId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public string CollisionWithEventMessage = "Not possible to subscribe. You are trying to " +
            "subscribe to two events that take place at the same time";

        public string GetSubscriptionType()
        {
            return this.GetType().ToString().Replace("ConventionManager.Models.", "");
        }

        public bool SubscriptionCollision(IList events, Event @event)
        {
            foreach (Event otherEvent in events)
            {
                foreach (var subscription in otherEvent.Subscriptions)
                {
                    if (subscription.UserId == this.UserId)
                    {
                        int startInEventA = DateTime.Compare(@event.StartDate, otherEvent.StartDate);
                        int startInEventB = DateTime.Compare(@event.StartDate, otherEvent.EndDate);

                        int endInEventA = DateTime.Compare(@event.EndDate, otherEvent.StartDate);
                        int endInEventB = DateTime.Compare(@event.EndDate, otherEvent.EndDate);

                        if ((startInEventA >= 0 && startInEventB <= 0) || (endInEventA >= 0 && endInEventB <= 0))
                        {
                            return false;
                        }
                        if ((startInEventA <= 0) && (endInEventB >= 0))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
