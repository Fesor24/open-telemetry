namespace Country.Domain.Countries
{
    public sealed record IsoCode
    {
        public IsoCode(string code)
        {
            Value = code.ToUpperInvariant();
        }

        public string Value { get; init; }
    }
}
