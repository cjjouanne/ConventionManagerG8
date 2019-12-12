using System;
using System.Collections.Generic;

namespace ConventionManager.Models
{
    public class EventFeedback
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int Overall { get; set; }
        public int Organization { get; set; }
        public int Attention { get; set; }
        public int RoomQuality { get; set; }
        public int Duration { get; set; } 
        public int WouldRecommend { get; set; }
        public string Other { get; set; }
        public int FoodQuality { get; set; }
        public DateTime Date { get; set; }
    }
}