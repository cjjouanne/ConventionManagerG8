using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConventionManager.Models
{
    public abstract class ExhibitorEvent : Event
    {
        [Required(ErrorMessage ="This type of event must have a topic")]
        public string Topic { get; set; }
    }
}