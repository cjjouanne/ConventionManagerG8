using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public abstract class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ConferenceId { get; set; }

        public int RoomId { get; set; }

        [Required(ErrorMessage = "An Room must have a name")]
        [StringLength(100, ErrorMessage = "Event name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "There must be a start date for this event!")]
        public DateTime startDate { get; set; }

        [Required(ErrorMessage = "There must be an end date for this event!")]
        public DateTime endtDate { get; set; }
        
    }
}