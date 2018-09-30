using DeliveryService.DL.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.API.Models.Routes
{
    public class RouteRequestModel
    {
        public int Limit { get; set; }
        [RouteOptionsRangeValidator()]
        public string[] OrderByParams { get; set; }
        public bool OrderByDescending { get; set; }
    }
    public sealed class RouteOptionsRangeValidator : ValidationAttribute
    {

        protected override ValidationResult IsValid(object inputArray, ValidationContext validationContext)
        {
            if (inputArray != null)
            {
                foreach (string inputString in (string[])inputArray)
                {
                    if (!RouteOptions.GetSortOptions().Contains(inputString))
                    {
                        return new ValidationResult(string.Format("{0} is not a valid option. Only the follow options are valid: {1}", inputString, RouteOptions.GetValidOptions()));
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
