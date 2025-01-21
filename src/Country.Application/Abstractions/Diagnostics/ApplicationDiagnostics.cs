using System.Diagnostics.Metrics;

namespace Country.Application.Abstractions.Diagnostics
{
    public static class ApplicationDiagnostics
    {
        // the service name
        private const string ServiceName = "Country.Api";
        public static readonly Meter Meter = new(ServiceName);

        // the instrument types
        public static Counter<long> NumbersRequestedCounter = Meter.CreateCounter<long>("numbers.requested",
            description: "Tracks the number of requests by phone no code");
    }
}
