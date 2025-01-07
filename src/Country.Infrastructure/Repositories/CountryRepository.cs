using Country.Domain.Countries;
using Country.Domain.Operators;
using Microsoft.EntityFrameworkCore;

namespace Country.Infrastructure.Repositories
{
    internal sealed class CountryRepository(ApplicationDbContext context) : 
        Repository<Domain.Countries.Country>(context), ICountryRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Domain.Countries.Country?> GetByCountryCodeAsync(string countryCode, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Domain.Countries.Country>()
                .Include(x => x.Operators)
                .FirstOrDefaultAsync(country => country.Code.Value == countryCode, cancellationToken);
        }
    }
}
