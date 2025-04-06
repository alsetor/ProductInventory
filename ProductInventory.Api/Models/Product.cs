using System.ComponentModel.DataAnnotations;

namespace ProductInventory.Api.Models
{
    public class Product : IIdentifier
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}