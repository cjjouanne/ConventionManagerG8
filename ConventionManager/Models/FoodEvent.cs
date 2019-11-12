using System.Collections.Generic;

namespace ConventionManager.Models
{
    public class FoodEvent : Event
    {
        public List<Food> Menu { get; set; }
    }
}