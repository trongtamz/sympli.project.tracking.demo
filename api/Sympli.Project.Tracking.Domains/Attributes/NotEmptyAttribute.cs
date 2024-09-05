using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Sympli.Project.Tracking.Domains.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NotEmptyAttribute : ValidationAttribute
    {
        public NotEmptyAttribute(string message = "") : base(message)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(base.ErrorMessage);
            }

            ValidationResult result = ((value is ICollection collection) ? ((collection.Count > 0) ? ValidationResult.Success : new ValidationResult(base.ErrorMessage)) : ((!(value is IEnumerable enumerable)) ? ValidationResult.Success : (enumerable.GetEnumerator().MoveNext() ? ValidationResult.Success : new ValidationResult(base.ErrorMessage))));

            return result;
        }
    }
}