using DistanceService.Domain;

namespace DistanceService.Infrastructure.Data
{
    public static class Mapper
    {
        public static AirportInfo ToDomain(CTeleport.Client.Models.AirportInfo info)
        {
            return info == null
                ? null
                : new AirportInfo(info.IataCode, new Point(info.Location.Lat, info.Location.Lon));
        }
    }
}
