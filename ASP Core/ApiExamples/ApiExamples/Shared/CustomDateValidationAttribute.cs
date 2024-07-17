using System.ComponentModel.DataAnnotations;

namespace ApiExamples.Shared
{
    public class CustomDateValidationAttribute : ValidationAttribute
    {

        private readonly DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        public string EndDate //named attribute params
        {
            get => _endDate.ToString();
            set => _endDate = DateTime.Parse(value);
        }

        public CustomDateValidationAttribute(string? startDate) // position attribute params 
        {
            if (startDate != null) _startDate = DateTime.Parse(startDate);
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            DateTime? date = value as DateTime?;
            if (date != null && (date >= _startDate && date <= _endDate))
                return ValidationResult.Success!;
            return new ValidationResult(ErrorMessage);

        }
    }
}
