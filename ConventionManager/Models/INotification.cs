using System;
namespace ConventionManager.Models
{
    public interface INotification
    {
        void SendNotificationToExhibitors(string message);

        void SendNotificationToAttendants(string message);
    }
}
