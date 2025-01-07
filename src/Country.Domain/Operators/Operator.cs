using Country.Domain.Abstractions;

namespace Country.Domain.Operators
{
    public sealed class Operator : Entity
    {
        private Operator()
        {
            
        }

        public Operator(string name, string code, Countries.Country country)
        {
            Name = name;
            Code = code;
            Country = country;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public long CountryId { get; private set; }
        public Countries.Country Country { get; private set; }
    }
}
