using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public class Sponsor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "A Sponsor must have a name")]
        public string Name { get; set; }

        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }
    }
}