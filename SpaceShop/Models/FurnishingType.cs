using System.ComponentModel.DataAnnotations;

namespace SpaceShop.Models
{
    public class FurnishingType : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}