using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public class EventCenter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ConferenceId { get; set; }
        [Required(ErrorMessage = "An Event Center must have a type")]
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
