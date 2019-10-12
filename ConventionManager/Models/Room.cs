using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventCenterId { get; set; }
        [Required(ErrorMessage = "An Room must have a name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "An Room must have a capacity of occupants")]
        public int Capacity { get; set; }
        public string Location { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
