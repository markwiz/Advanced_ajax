using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace AdvancedAjax.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(75)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(75)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-Mail is not Valid")]
        public string EmailId { get; set; } = string.Empty;

        [Required]
        [DisplayName("Country")]
        [NotMapped]
        public int CountryId { get; set; }

        [Required]
        [ForeignKey("City")]
        [DisplayName("City")]
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        [MaxLength(555)]
        public string PhotoUrl { get; set; } = string.Empty;

        [Display(Name = "Profile Photo")]
        [NotMapped]
        public IFormFile? ProfilePhoto { get; set; }

        [NotMapped]
        public string BreifPhotoName { get; set; } = string.Empty;
    }
} 