namespace AdvancedAjax.Models;

 public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(3)]
        public required string Code { get; set; }

        [Required]
        [MaxLength(75)]
        public required string Name { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; } 

        public virtual Country? Country { get; set; } 
    }