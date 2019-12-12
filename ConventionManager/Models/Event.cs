using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConventionManager.Data;

namespace ConventionManager.Models
{
    public abstract class Event : IStatistics<EventFeedback, EventQualityStatistics>
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

        public string CannotDeleteEventMessage = "This event can't be deleted because there " +
                                                    "is at least someone subscribed to it.";

        public string NoModeratorYetMessage = "This Event Needs a Moderator before " +
            "emptying its capacity. You could join as one if you wish to.";

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

            if (startInConference < 0 || endInConference > 0)
            {
                return false;
            }
            else if (currentDate < 0)
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

        public EventQualityStatistics GetRating(List<EventFeedback> feedbacks)
        {
            int totalPoints = 0;
            int overall = 0;
            int organization = 0;
            int attention = 0;
            int roomQuality = 0;
            int duration = 0;
            int wouldRecommend = 0;
            int totalFeedbacks = feedbacks.Count;
            foreach (var feedback in feedbacks)
            {
                overall += feedback.Overall;
                organization += feedback.Organization;
                attention += feedback.Attention;
                roomQuality += feedback.RoomQuality;
                duration += feedback.Duration;
                wouldRecommend += feedback.WouldRecommend;
                totalPoints += feedback.Overall + feedback.Organization +
                    feedback.Attention + feedback.RoomQuality +
                    feedback.Duration + feedback.WouldRecommend;
            }

            var eventQualityStatistic = new EventQualityStatistics(totalFeedbacks);
            eventQualityStatistic.SetTotal(totalPoints);
            eventQualityStatistic.SetOverall(overall);
            eventQualityStatistic.SetOrganizationOrPreparation(organization);
            eventQualityStatistic.SetAttentionOrAttitude(attention);
            eventQualityStatistic.SetRoomQualityOrVoice(roomQuality);
            eventQualityStatistic.SetDuartionOrConnection(duration);
            eventQualityStatistic.SetWouldRecommend(wouldRecommend);

            return eventQualityStatistic;
        }
    }
}