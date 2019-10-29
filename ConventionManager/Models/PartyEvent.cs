namespace ConventionManager.Models
{
    public class PartyEvent : Event
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