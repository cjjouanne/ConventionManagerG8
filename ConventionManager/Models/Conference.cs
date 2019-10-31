using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public class Conference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "A Conference must have a name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "There must be a start date for this conference!")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "There must be a start date for this conference!")]
        public DateTime EndDate { get; set; }

        public ICollection<Sponsor> Sponsors { get; set; }

        public ICollection<Event> Events { get; set; }

        public int EventCenterId { get; set; }
        public EventCenter EventCenter { get; set; }

        public string ConferenceCollisionMessage = "Conference not created. The " +
            "Conference collides with another Conference in the Event Center";

        public string OutOfDateMessage = "Conference not created. The entered " +
            "dates already happened or are swapped!";


        public bool CheckCollisionWithConference(EventCenter eventCenter)
        {
            foreach (Conference conference in eventCenter.Conferences)
            {
                int startInConferenceA = DateTime.Compare(this.StartDate, conference.StartDate);
                int startInConferenceB = DateTime.Compare(this.StartDate, conference.EndDate);

                int endInConferenceA = DateTime.Compare(this.EndDate, conference.StartDate);
                int endInConferenceB = DateTime.Compare(this.EndDate, conference.EndDate);

                if ((startInConferenceA >= 0 && startInConferenceB <= 0) || (endInConferenceA >= 0 && endInConferenceB <= 0))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckIfOutOfDate()
        {
            int currentDate = DateTime.Compare(this.StartDate, DateTime.Now);
            int startAndEnd = DateTime.Compare(this.StartDate, this.EndDate);

            if (currentDate <= 0)
            {
                return false;
            }
            if (startAndEnd >= 0)
            {
                return false;
            }
            return true;
        }
    }
}