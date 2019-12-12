using System;
namespace ConventionManager.Models
{
    public interface IQualityStatisticsBuilder
    {
        void SetTotal(int stat);
        void SetOverall(int stat);
        void SetOrganizationOrPreparation(int stat);
        void SetAttentionOrAttitude(int stat);
        void SetRoomQualityOrVoice(int stat);
        void SetDuartionOrConnection(int stat);
        void SetExtraStat(int stat);
    }
}
