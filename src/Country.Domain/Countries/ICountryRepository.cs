namespace Country.Domain.Countries
{
    public interface ICountryRepository
    {
        Task<Country?> GetByCountryCodeAsync(string countryCode, CancellationToken cancellationToken = default);
    }
}
