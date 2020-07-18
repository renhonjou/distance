using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DistanceService.Domain.Services;
using MediatR;

namespace DistanceService.Application.Queries
{
    public class GetDistanceBetweenAirportsQuery : IRequest<Result<double>>
    {
        /// <summary>
        /// IATA Airport Code. Departure
        /// </summary>
        public string From { get; }
        /// <summary>
        /// IATA Airport Code. Arrival
        /// </summary>
        public string To { get; }

        public GetDistanceBetweenAirportsQuery(string from, string to)
        {
            From = from;
            To = to;
        }

        public class GetDistanceBetweenAirportsQueryHandler : IRequestHandler<GetDistanceBetweenAirportsQuery, Result<double>>
        {
            private readonly IAirportService _airportService;
            private readonly IDistanceService _distanceService;

            public GetDistanceBetweenAirportsQueryHandler(IAirportService airportService,
                IDistanceService distanceService)
            {
                _airportService = airportService ?? throw new ArgumentNullException(nameof(airportService));
                _distanceService = distanceService ?? throw new ArgumentNullException(nameof(distanceService));
            }

            public async Task<Result<double>> Handle(GetDistanceBetweenAirportsQuery request,
                CancellationToken cancellationToken)
            {
                var fromTask = Task.Run(() => _airportService.GetAirport(request.From), cancellationToken);
                var toTask = Task.Run(() => _airportService.GetAirport(request.To), cancellationToken);
                await Task.WhenAll(fromTask, toTask);

                var fromAirport = fromTask.Result;
                var toAirport = toTask.Result;
                if (fromAirport?.Location == null || toAirport?.Location == null)
                {
                    return Result.Failure<double>("Couldn't get an additional information about airports.");
                }

                var metersDistance = _distanceService.GetDistanceInMeters(fromAirport.Location, toAirport.Location);
                var miles = _distanceService.ToMiles(metersDistance);

                return Result.Success(miles);
            }
        }
    }
}
