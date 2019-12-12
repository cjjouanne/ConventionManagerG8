using System;
namespace ConventionManager.Models
{
    public class EventQualityStatistics : IQualityStatisticsBuilder
    {
        public int Total { get; set; }
        public int Overall { get; set; }
        public int Organization { get; set; }
        public int Attention { get; set; }
        public int RoomQuality { get; set; }
        public int Duration { get; set; }
        public int WouldRecommend { get; set; }
        public int FoodQuality { get; set; }
        private int _totalFeedbacks;

        public EventQualityStatistics(int totalFeedbacks)
        {
            _totalFeedbacks = totalFeedbacks;
        }

        public void SetOverall(int stat)
        {
            Overall = stat / _totalFeedbacks;
        }

        public void SetOrganizationOrPreparation(int stat)
        {
            Organization = stat / _totalFeedbacks;
        }

        public void SetAttentionOrAttitude(int stat)
        {
            Attention = stat / _totalFeedbacks;
        }

        public void SetRoomQualityOrVoice(int stat)
        {
            RoomQuality = stat / _totalFeedbacks;
        }

        public void SetDuartionOrConnection(int stat)
        {
            Duration = stat / _totalFeedbacks;
        }

        public void SetExtraStat(int stat)
        {
        }

        public void SetWouldRecommend(int stat)
        {
            WouldRecommend = stat / _totalFeedbacks;
        }

        public void SetTotal(int stat)
        {
            Total = stat / 5;
        }
    }
}
