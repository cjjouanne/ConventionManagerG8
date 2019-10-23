using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConventionManager.Models
{
    public class Food
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FoodEventId { get; set; }
        public FoodEvent FoodEvent { get; set; }

        [Required(ErrorMessage = "Food must have a name")]
        [StringLength(50, ErrorMessage = "Food name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Food must have a type")]
        [StringLength(50, ErrorMessage = "Food type cannot be longer than 50 characters.")]
        public string TypeOfFood { get; set; }

        [Required(ErrorMessage = "Food has to include a small description")]
        public string Description { get; set; }
    }
}