using System;
using System.Threading.Tasks;
using CTeleport.Client.Interfaces;
using DistanceService.Domain;
using DistanceService.Domain.Services;

namespace DistanceService.Infrastructure.Data.Services
{
    public class AirportService : IAirportService
    {
        private readonly ICTeleportClient _client;

        public AirportService(ICTeleportClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<AirportInfo> GetAirport(string iataCode)
        {
            var result = await _client.GetAirportInfo(iataCode);
            return Mapper.ToDomain(result);
        }
    }
}
