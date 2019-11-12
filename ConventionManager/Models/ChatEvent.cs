using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConventionManager.Models
{
    public class ChatEvent : ExhibitorEvent
    {
        public string ModeratorId { get; set; }
    }
}