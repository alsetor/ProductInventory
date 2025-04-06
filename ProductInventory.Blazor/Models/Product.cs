using System.ComponentModel.DataAnnotations;

namespace ProductInventory.Blazor.Models;

public class Product
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(128, ErrorMessage = "Name cannot exceed 128 characters.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1024, ErrorMessage = "Description cannot exceed 1024 characters.")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public int Quantity { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    public decimal Price { get; set; }
}
