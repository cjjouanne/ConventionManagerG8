using System;
namespace ConventionManager.Models
{
    public class ConferenceAndSponsor
    {
        public Conference Conference { get; set; }
        public Sponsor Sponsor { get; set; } 
        public Event Event { get; set; }
    }
}
