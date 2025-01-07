using Country.Domain.Abstractions;

namespace Country.Domain.Countries
{
    public static class CountryErrors
    {
        public static NotFound NotFound = new("Country.Notfound", "Country not found");
    }
}
