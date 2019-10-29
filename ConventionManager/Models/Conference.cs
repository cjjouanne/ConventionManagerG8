using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public class Conference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "A Conference must have a name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "There must be a start date for this conference!")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "There must be a start date for this conference!")]
        public DateTime EndDate { get; set; }

        public ICollection<Sponsor> Sponsors { get; set; }

        public ICollection<Event> Events { get; set; }

        public int EventCenterId { get; set; }
        public EventCenter EventCenter { get; set; }
    }
}