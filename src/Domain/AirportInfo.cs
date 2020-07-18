namespace DistanceService.Domain
{
    public class AirportInfo
    {
        public AirportInfo(string iataCode, Point location)
        {
            IataCode = iataCode;
            Location = location;
        }

        public string IataCode { get; }
        public Point Location { get; }
    }
}
