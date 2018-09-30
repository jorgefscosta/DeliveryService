using System.ComponentModel.DataAnnotations;

namespace DeliveryService.API.Models.Routes
{
    public class ShipsToRequestModel
    {
        [Required]
        [NonNegativeIntValidator()]
        public int Time { get; set; }
        [Required]
        [NonNegativeIntValidator()]
        public int Cost { get; set; }
        public sealed class NonNegativeIntValidator : ValidationAttribute
        {
            protected override ValidationResult IsValid(object inputNumber, ValidationContext validationContext)
            {
                var value = (int)inputNumber;
                return value<0 ? new ValidationResult("Must be non negative") : ValidationResult.Success;
            }
        }
    }
}
