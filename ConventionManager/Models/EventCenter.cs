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

        [Required(ErrorMessage = "An Event Center must have a name")]
        [StringLength(50, ErrorMessage = "Event Center name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "An Event Center must have a type")]
        [StringLength(30, ErrorMessage = "Event Center type cannot be longer than 30 characters.")]
        public string Type { get; set; }

        public string Location { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public ICollection<Conference> Conferences { get; set; }

        public Dictionary<string, string> AvailableTypes = new Dictionary<string, string>()
        {
            {"Hotel", "Hotel" },
            {"University", "University" },
            {"Event Center", "EventCenter" },
            {"Theater", "Theater" },
            {"Other", "Other" },
        };
    }
}
