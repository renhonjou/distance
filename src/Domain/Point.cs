namespace DistanceService.Domain
{
    public class Point
    {
        public Point(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }

        public double Lat { get; }
        public double Lon { get; }
    }
}
