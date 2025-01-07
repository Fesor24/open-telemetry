using System.Text.RegularExpressions;

namespace Country.Domain.Countries
{
    public sealed record CountryCode
    {
        public CountryCode(string code)
        {
            string pattern = @"^\d{2,3}$";

            Regex regex = new(pattern);

            if (!regex.IsMatch(code))
                throw new ApplicationException("Invalid country code");

            Value = code;
        }

        public string Value { get; init; }
    }
}
