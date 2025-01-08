using Country.Application.Abstractions.Cache;

namespace Country.Application.Countries.GetCountryByPhoneNo
{
    public sealed record GetCountryByPhoneNoQuery(string PhoneNo) :
        ICachedQuery<GetCountryByPhoneNoResponse>
    {
        public string Key => $"Country-{PhoneNo}";
        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}
