namespace Country.Domain.Countries
{
    public sealed record IsoCode
    {
        public IsoCode(string code)
        {
            Code = code.ToUpperInvariant();
        }

        public string Code { get; init; }
    }
}
