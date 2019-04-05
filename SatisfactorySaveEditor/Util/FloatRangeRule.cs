using System;
using System.Globalization;
using System.Windows.Controls;

namespace SatisfactorySaveEditor.Util
{
    public class FloatRangeRule : ValidationRule
    {
        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var number = 0f;
            if (!(value is string numberString)) return new ValidationResult(false, "Input value is not a string.");
            if (numberString.EndsWith(cultureInfo.NumberFormat.NumberDecimalSeparator)) return new ValidationResult(false, "Input value cannot end with a decimal separator.");

            try
            {
                if (numberString.Length > 0) number = float.Parse(numberString, cultureInfo);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if (number > MaxValue || number < MinValue) return new ValidationResult(false,$"Number is not in range: {MinValue} - {MaxValue}.");
            return ValidationResult.ValidResult;
        }
    }
}
