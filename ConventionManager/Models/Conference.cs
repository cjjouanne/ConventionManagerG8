using System.Collections.Generic;

namespace ConventionManager.Models
{
    public class Conference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<Sponsor> Sponsors { get; set; }
    }
}