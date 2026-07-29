using System.ComponentModel.DataAnnotations;

namespace soccer.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Number { get; set; } 

        [Required]
        [StringLength(50)]
        public string Position { get; set; } 

        public string ImageUrl { get; set; } 

        
        public double Rating { get; set; }

        public int WeeklyGoals { get; set; }
        public int WeeklyAssists { get; set; }

       
        public int TotalGoals { get; set; }
        public int TotalAssists { get; set; }


        public string TeamName { get; set; }
    }
}