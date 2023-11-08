using System.ComponentModel.DataAnnotations;

namespace SpaceShop.Models
{
    public class PropertyType : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}