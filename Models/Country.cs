namespace AdvancedAjax.Models
{
    public class Country
    {
        [Key]        
        public int Id { get; set; }

        [Required]
        [MaxLength(4)]
        public required string Code { get; set; }

        [Required]
        [MaxLength(75)]
        public required string Name { get; set; }

        [MaxLength(75)]
        public string? CurrencyName { get; set; }
    }
}