using System.Threading.Tasks;

namespace DistanceService.Domain.Services
{
    public interface IAirportService
    {
        Task<AirportInfo> GetAirport(string iataCode);
    }
}
