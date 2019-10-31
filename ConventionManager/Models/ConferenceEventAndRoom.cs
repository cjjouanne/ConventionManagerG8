using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConventionManager.Models
{
    public class ConferenceEventAndRoom
    {
        public EventCenter EventCenter { get; set; }
        public Conference Conference { get; set; }
        public Event Event { get; set; }
        public Room Room { get; set; }

        public Dictionary<string, string> EventTypes = new Dictionary<string, string>()
        {
            {"Party Event", "PartyEvent" },
            {"Food Event", "FoodEvent" },
            {"Chat Event", "ChatEvent" },
            {"Practical Session Event", "PracticalSessionsEvent" },
            {"Talk Event", "TalkEvent" },
        };
    }
}
