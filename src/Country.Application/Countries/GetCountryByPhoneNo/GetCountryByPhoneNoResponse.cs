namespace Country.Application.Countries.GetCountryByPhoneNo
{
    public sealed class GetCountryByPhoneNoResponse
    {
        public string Number { get; set; }
        public Country Country { get; set; }
    };

    public sealed class Country
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public string CountryIso { get; set; }
        public IReadOnlyList<CountryOperator> CountryDetails { get; set; } = [];
    };


    public sealed class CountryOperator
    {
        public string Operator { get; set; }
        public string OperatorCode { get; set; }
    }
}
