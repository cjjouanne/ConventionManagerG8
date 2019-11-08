using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConventionManager.Data;

namespace ConventionManager.Models
{
    public abstract class Event : INotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "An Room must have a name")]
        [StringLength(100, ErrorMessage = "Event name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "There must be a start date for this event!")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "There must be an end date for this event!")]
        public DateTime EndDate { get; set; }

        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }

        public string OutOfRangeMessage = "Event not created. The entered " +
            "dates already happened, are out of range with the Conference " +
            "or are swapped!";

        public string CollisionWithEventMessage = "Event not created. The " +
            "Event collides with another Event in the Conference or in a Room";


        // Gets the name of the event type with GetType() method
        public string GetEventType()
        {
            return this.GetType().ToString().Replace("ConventionManager.Models.", "");
        }


        public bool CheckDateTime(Conference conference)
        {
            int startInConference = DateTime.Compare(this.StartDate, conference.StartDate);
            int endInConference = DateTime.Compare(this.EndDate, conference.EndDate);
            int currentDate = DateTime.Compare(this.StartDate, DateTime.Now);
            int startAndEnd = DateTime.Compare(this.StartDate, this.EndDate);

            if (startInConference <= 0 || endInConference >= 0)
            {
                return false;
            }
            else if (currentDate <= 0)
            {
                return false;
            }
            else if (startAndEnd >= 0)
            {
                return false;
            }
            return true;
        }


        public bool CheckCollisionWithEvent(IList events)
        {
            foreach (Event @event in events)
            {
                int startInEventA = DateTime.Compare(this.StartDate, @event.StartDate);
                int startInEventB = DateTime.Compare(this.StartDate, @event.EndDate);

                int endInEventA = DateTime.Compare(this.EndDate, @event.StartDate);
                int endInEventB = DateTime.Compare(this.EndDate, @event.EndDate);

                if (this.RoomId == @event.RoomId)
                {
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
            return true;
        }

        public abstract void SendNotificationToAttendants(string message);
        public abstract void SendNotificationToExhibitors(string message);
    }
}