using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConventionManager.Models
{
    public class TalkEvent : ExhibitorEvent
    {
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