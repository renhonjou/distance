using System.Threading.Tasks;
using CTeleport.Client.Models;

namespace CTeleport.Client.Interfaces
{
    public interface ICTeleportClient
    {
        Task<AirportInfo> GetAirportInfo(string iataCode);
    }
}
