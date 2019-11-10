using System;
using System.Collections.Generic;

namespace ConventionManager.Models
{
    public class SearchResults
    {
        public IEnumerable<EventCenter> EventCenters { get; set; }
        public IEnumerable<Conference> Conferences { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public ApplicationUser User { get; set; }
    }
}
