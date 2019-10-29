using System.Collections.Generic;

namespace ConventionManager.Models
{
    public class FoodEvent : Event
    {
        public List<Food> Menu { get; set; }

        public override void SendNotificationToAttendants(string message)
        {
            throw new System.NotImplementedException();
        }

        public override void SendNotificationToExhibitors(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}