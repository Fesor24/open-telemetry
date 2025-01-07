using Country.Domain.Abstractions;
using Country.Domain.Operators;

namespace Country.Domain.Countries
{
    public sealed class Country : Entity
    {
        private Country()
        {
            
        }

        public Country(string name, CountryCode countryCode, IsoCode isoCode)
        {
            Name = name;
            Code = countryCode;
            IsoCode = isoCode;
        }

        public string Name { get; private set; }
        public CountryCode Code { get; private set; }
        public IsoCode IsoCode { get; private set; }
        public ICollection<Operator> Operators { get; } = [];
    }
}
