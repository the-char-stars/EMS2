using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EMS.Validation
{
    class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }

    class OnlyNumbersValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return (value ?? "").ToString().All(char.IsNumber)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Numeric input only.");
        }
    }

    class OnlyLettersValidationRule : ValidationRule
    {
        bool isValid(char x)
        {
            return x == '-' || char.IsLetter(x);
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return (value ?? "").ToString().All(isValid)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Non-numeric input only.");
        }
    }

    class NumbersAndLettersValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return (value ?? "").ToString().All(char.IsLetterOrDigit)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Field is required.");
        }
    }

    class SingleCharacterValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string s = (value ?? "").ToString();
            return s.All(char.IsLetter) && s.Length <= 1
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Single character only.");
        }
    }

    class HealthCardValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new Regex("^([0-9]{10}[a-zA-Z]{2}$)").IsMatch((value ?? "").ToString())
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Invalid Health Card Format.");
        }
    }

    class PostalCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new Regex("^[A-Z][0-9][A-Z][- ]?[0-9][A-Z][0-9]$").IsMatch((value ?? "").ToString().ToUpper())
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Invalid Postal Code Format.");
        }
    }

    public class FutureDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out DateTime time)) return new ValidationResult(false, "Invalid date");

            return time.Date <= DateTime.Now.Date
                ? new ValidationResult(false, "Future date required")
                : ValidationResult.ValidResult;
        }
    }

    public class PastDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out DateTime time)) return new ValidationResult(false, "Invalid date");

            return time.Date >= DateTime.Now.Date
                ? new ValidationResult(false, "Past date required")
                : ValidationResult.ValidResult;
        }
    }

}
