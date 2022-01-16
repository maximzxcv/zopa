using System.ComponentModel.DataAnnotations;

namespace Zopa.Console.Validation
{
    public class MultipleToAttribute : ValidationAttribute
    { 
        private readonly int _denominator;

        public MultipleToAttribute(int denominator)
        {
            _denominator = denominator;
        }

        public override bool IsValid(object value)
        {
            if (!int.TryParse((string) value, out var intValue))
                return false;

            return intValue % _denominator == 0;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field {name} must be multiple to {_denominator}.";
        }
    }
}