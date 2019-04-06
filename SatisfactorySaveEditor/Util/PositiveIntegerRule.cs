using System;
using System.Globalization;
using System.Windows.Controls;

namespace SatisfactorySaveEditor.Util
{
    public class PositiveIntegerRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var number = 0;
            if (!(value is string numberString)) return new ValidationResult(false, "Input value is not a string.");

            try
            {
                if (numberString.Length > 0) number = int.Parse(numberString, cultureInfo);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if (number < 0) return new ValidationResult(false,"Number is not in range: 0 - Infinity.");
            return ValidationResult.ValidResult;
        }
    }
}
