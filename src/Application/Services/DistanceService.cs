using System;
using DistanceService.Domain;
using DistanceService.Domain.Services;

namespace DistanceService.Application.Services
{
    public class DistanceService : IDistanceService
    {
        public double GetDistanceInMeters(Point point1, Point point2)
        {
            if (point1 == null) throw new ArgumentNullException(nameof(point1));
            if (point2 == null) throw new ArgumentNullException(nameof(point2));

            return GetDistanceInMeters(point1.Lat, point1.Lon, point2.Lat, point2.Lon);
        }

        public double GetDistanceInMeters(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371000; //meters
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var dist = earthRadius * c;
            return dist;
        }

        public double ToMiles(double meters)
        {
            if (meters < 0) throw new ArgumentOutOfRangeException(nameof(meters));

            return meters * 0.0006213712;
        }

        private double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
