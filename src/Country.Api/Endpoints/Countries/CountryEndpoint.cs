using System.Net;
using Country.Application.Countries.GetCountryByPhoneNo;
using Country.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Country.Api.Endpoints.Countries
{
    public static class CountryEndpoint
    {
        public static void RegisterEndpoint(IEndpointRouteBuilder app)
        {
            var routeGroup = app.MapGroup("api/country")
                .WithTags("Country")
                .ProducesProblem((int)HttpStatusCode.BadRequest)
                .ProducesProblem((int)HttpStatusCode.NotFound);

            routeGroup.MapGet("{phoneNo}", GetCountryByPhoneNoAsync);
        }

        private static async Task<IResult> GetCountryByPhoneNoAsync(string phoneNo, ISender sender, 
            CancellationToken cancellationToken)
        {
            var query = new GetCountryByPhoneNoQuery(phoneNo);

            var result = await sender.Send(query, cancellationToken);

            return result.Match(value => Results.Ok(value), HandleError);
        }

        private static IResult HandleError(Error error)
        {
            if(error.GetType() == typeof(BadRequest))
                return Results.BadRequest(new ProblemDetails
                {
                    Detail = error.Message,
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = error.Code
                });

            else if (error.GetType() == typeof(NotFound))
                return Results.NotFound(new ProblemDetails
                {
                    Detail = error.Message,
                    Status = (int)HttpStatusCode.NotFound,
                    Title = error.Code
                });

            else return Results.BadRequest(new ProblemDetails
            {
                Detail = error.Message,
                Status = (int)HttpStatusCode.BadRequest,
                Title = error.Code
            });
        }
    }
}
