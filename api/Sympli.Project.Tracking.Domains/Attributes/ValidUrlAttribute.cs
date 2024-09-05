using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Sympli.Project.Tracking.Domains.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class ValidUrlAttribute : ValidationAttribute
    {
        public ValidUrlAttribute(string message = "") : base(message)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            if (Uri.TryCreate(value.ToString(), UriKind.Absolute, out _))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName, value));
        }

        private string FormatErrorMessage(string name, object value)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"The value of field {name}: {value} is not valid."
                : string.Format(CultureInfo.CurrentCulture, ErrorMessage, name, value);
        }
    }
}