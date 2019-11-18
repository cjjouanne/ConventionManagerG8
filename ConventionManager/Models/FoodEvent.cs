using System.Collections.Generic;

namespace ConventionManager.Models
{
    public class FoodEvent : Event
    {
        public ICollection<Food> Menu { get; set; }
    }
}