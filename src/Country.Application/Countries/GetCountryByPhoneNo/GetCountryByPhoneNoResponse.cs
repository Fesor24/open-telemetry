namespace Country.Application.Countries.GetCountryByPhoneNo
{
    public sealed record GetCountryByPhoneNoResponse(
        string Number,
        Country Country
        );

    public sealed record Country(
        string CountryCode,
        string Name,
        string CountryIso,
        IReadOnlyList<CountryOperator> CountryDetails
        );

    public sealed record CountryOperator(
        string Operator,
        string OperatorCode
        );


}
