using System.Diagnostics;
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

            //always set primitive types...otherwise we might get into exceptions...
            Activity.Current?.SetTag("phoneno", request.PhoneNo);
            Activity.Current?.SetTag("phoneno.prefix", code);

            var country = await _countryRepository.GetByCountryCodeAsync(code, cancellationToken);

            if (country is null) return CountryErrors.NotFound;

            return new GetCountryByPhoneNoResponse
            {
                Number = request.PhoneNo,
                Country = new Country
                {
                    CountryCode = country.Code.Value,
                    Name = country.Name,
                    CountryIso = country.IsoCode.Value,
                    CountryDetails = country.Operators
                        .Select(op => new CountryOperator
                        {
                            Operator = op.Name,
                            OperatorCode = op.Code
                        }).ToList()
                }
            };
        }
    }
}
