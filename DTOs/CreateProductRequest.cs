using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs
{
    public class CreateProductRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Please enter a product code, such as P006.")]
        [StringLength(20, ErrorMessage = "The product code cannot exceed 20 characters.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a product name.")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "The product name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please add a short description of the product.")]
        [StringLength(500, ErrorMessage = "The description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(
            typeof(decimal),
            "0.01",
            "1000000",
            ErrorMessage = "Enter a price greater than 0 and no more than 1,000,000.")]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Name) &&
                Name.Contains("Premium", StringComparison.OrdinalIgnoreCase) &&
                Price < 100)
            {
                yield return new ValidationResult(
                    "Premium products must cost at least 100.",
                    new[] { nameof(Price) });
            }
        }
    }
}