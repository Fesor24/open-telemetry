using Country.Application.Abstractions.Messaging;

namespace Country.Application.Countries.GetCountryByPhoneNo
{
    public sealed record GetCountryByPhoneNoQuery(string PhoneNo) :
        IQuery<GetCountryByPhoneNoResponse>;
}
