using System;
namespace ConventionManager.Models
{
    public class ExhibitorQualityStatistics : IQualityStatisticsBuilder
    {
        public int Total { get; set; }
        public int Overall { get; set; }
        public int Preparation { get; set; }
        public int Attitude { get; set; }
        public int Voice { get; set; }
        public int Connection { get; set; }
        private int _totalFeedbacks;

        public ExhibitorQualityStatistics(int totalFeedbacks)
        {
            _totalFeedbacks = totalFeedbacks;
        }

        public void SetOverall(int stat)
        {
            Overall = stat / _totalFeedbacks;
        }

        public void SetOrganizationOrPreparation(int stat)
        {
            Preparation = stat / _totalFeedbacks;
        }

        public void SetAttentionOrAttitude(int stat)
        {
            Attitude = stat / _totalFeedbacks;
        }

        public void SetRoomQualityOrVoice(int stat)
        {
            Voice = stat / _totalFeedbacks;
        }

        public void SetDuartionOrConnection(int stat)
        {
            Connection = stat / _totalFeedbacks;
        }

        public void SetExtraStat(int stat)
        {
        }

        public void SetTotal(int stat)
        {
            Total = stat / (_totalFeedbacks * 5);
        }
    }
}
