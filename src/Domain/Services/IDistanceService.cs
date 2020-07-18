namespace DistanceService.Domain.Services
{
    public interface IDistanceService
    {
        double GetDistanceInMeters(Point point1, Point point2);
        double GetDistanceInMeters(double lat1, double lon1, double lat2, double lon2);
        double ToMiles(double meters);
    }
}
