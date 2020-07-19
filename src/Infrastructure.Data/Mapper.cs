using DistanceService.Domain;

namespace DistanceService.Infrastructure.Data
{
    public static class Mapper
    {
        public static AirportInfo ToDomain(CTeleport.Client.Models.AirportInfo info)
        {
            if (info == null) return null;

            var location = info.Location != null ? new Point(info.Location.Lat, info.Location.Lon) : null;
            return new AirportInfo(info.IataCode, location);
        }
    }
}
