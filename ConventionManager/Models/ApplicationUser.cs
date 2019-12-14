using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using Microsoft.AspNetCore.Identity;

namespace ConventionManager.Models
{
    public class ApplicationUser : IdentityUser, IStatistics<ExhibitorFeedback, ExhibitorQualityStatistics>
    {
        // UserName and Phone number already in IdentityUser class
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CurriculumUrl { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        public ExhibitorQualityStatistics GetRating(List<ExhibitorFeedback> feedbacks)
        {
            int totalPoints = 0;
            int overall = 0;
            int organization = 0;
            int attention = 0;
            int roomQuality = 0;
            int duration = 0;
            int totalFeedbacks = feedbacks.Count;
            foreach (var feedback in feedbacks)
            {
                overall += feedback.Overall;
                organization += feedback.Preparation;
                attention += feedback.Attitude;
                roomQuality += feedback.Voice;
                duration += feedback.Connection;
                totalPoints += feedback.Overall + feedback.Preparation +
                    feedback.Attitude + feedback.Voice +
                    feedback.Connection;
            }

            if (totalFeedbacks == 0)
            {
                totalFeedbacks = 1;
            }
            var exhibitorQualityStatistics = new ExhibitorQualityStatistics(totalFeedbacks);
            exhibitorQualityStatistics.SetTotal(totalPoints);
            exhibitorQualityStatistics.SetOverall(overall);
            exhibitorQualityStatistics.SetOrganizationOrPreparation(organization);
            exhibitorQualityStatistics.SetAttentionOrAttitude(attention);
            exhibitorQualityStatistics.SetRoomQualityOrVoice(roomQuality);
            exhibitorQualityStatistics.SetDuartionOrConnection(duration);

            return exhibitorQualityStatistics;
        }
    }
}
