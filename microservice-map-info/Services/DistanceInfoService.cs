using GoogleMapInfo;
using Grpc.Core;
using microservice_map_info.Protos;
using Prometheus;

namespace microservice_map_info.Services
{
    public class DistanceInfoService : DistanceInfo.DistanceInfoBase
    {

        // Aggiungi metriche custom
        private static readonly Counter googleApiCount = Metrics.CreateCounter("google_api_calls_total", "Number of times Google geolocation api is called.");
        private static readonly Counter googleApiLocations = Metrics.CreateCounter("google_api_locations", "Google Maps from and to locations.");

        private readonly ILogger<DistanceInfoService> _logger;
        private readonly GoogleDistanceApi _googleDistanceApi;
        public DistanceInfoService(ILogger<DistanceInfoService> logger, GoogleDistanceApi googleDistanceApi)
        {
            _logger = logger;
            _googleDistanceApi = googleDistanceApi;
        }

        public override async Task<DistanceData> GetDistance(Cities cities, ServerCallContext context)
        {
            var totalMiles = "0";

            // Issue metrics
            googleApiCount.Inc();
            googleApiLocations.WithLabels(cities.OriginCity, cities.DestinationCity).Inc();

            var distanceData = await _googleDistanceApi.GetMapDistance(cities.OriginCity, cities.DestinationCity);
            foreach (var distanceDataRow in distanceData[0].rows)
            {
                foreach (var element in distanceDataRow.elements)
                {
                    totalMiles = element.distance.text;
                }
            }
            return new DistanceData { Miles = totalMiles };
        }
    }
    
}
