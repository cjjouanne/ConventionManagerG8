﻿using System;
using System.Collections.Generic;

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
            {"Practical Session Event", "PracticalSessionEvent" },
            {"Talk Event", "TalkEvent" },
        };
    }
}
