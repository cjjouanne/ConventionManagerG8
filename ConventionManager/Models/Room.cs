using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ConventionManager.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "An Room must have a name")]
        [StringLength(50, ErrorMessage = "Room name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "An Room must have a capacity of occupants")]
        public int Capacity { get; set; }

        public string Location { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public ICollection<Event> Events { get; set; }

        public int EventCenterId { get; set; }
        public EventCenter EventCenter { get; set; }

        public int GetVacancies(Event @event)
        {
            return this.Capacity - @event.Subscriptions.Count();
        }

        public string NoMoreVacanciesMessage = "This event has no more vacancies.";
    }
}
