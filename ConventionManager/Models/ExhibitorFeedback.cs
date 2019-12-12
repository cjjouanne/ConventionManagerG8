using System;

namespace ConventionManager.Models
{
    public class ExhibitorFeedback
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string ExhibitorId { get; set; }
        public ApplicationUser Exhibitor { get; set; }
        public int Overall { get; set; }
        public int Preparation { get; set; }
        public int Attitude { get; set; }
        public int Voice { get; set; }
        public int Connection { get; set; }
        public string Other { get; set; }
        public DateTime Date { get; set; }
    }
}
