using Country.Application.Abstractions.Messaging;
using Country.Domain.Abstractions;
using Country.Domain.Countries;

namespace Country.Application.Countries.GetCountryByPhoneNo
{
    internal sealed class GetCountryByPhoneNoQueryHandler :
        IQueryHandler<GetCountryByPhoneNoQuery, GetCountryByPhoneNoResponse>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountryByPhoneNoQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<GetCountryByPhoneNoResponse>> Handle(GetCountryByPhoneNoQuery request, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNo))
                return Error.BadRequest("Bad.Request", "Phone no can not be empty or null");

            if (request.PhoneNo.Length < 4)
                return Error.BadRequest("Bad.Request", "Phone no must be at least 4 characters");

            string code = request.PhoneNo[..3];

            var country = await _countryRepository.GetByCountryCodeAsync(code, cancellationToken);

            if (country is null) return CountryErrors.NotFound;

            return new GetCountryByPhoneNoResponse(
                request.PhoneNo,
                new Country(
                    country.Code.Value,
                    country.Name,
                    country.IsoCode.Value,
                    country.Operators
                        .Select(op => new CountryOperator(op.Name, op.Code)).ToList()
                    )
                );
        }
    }
}
