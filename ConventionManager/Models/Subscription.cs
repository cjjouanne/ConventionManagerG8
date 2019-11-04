using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public abstract class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ConferenceId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public bool CheckIfEventAtSameTime()
        {
            return false;
        }
    }
}
