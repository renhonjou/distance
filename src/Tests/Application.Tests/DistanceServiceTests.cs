using System;
using DistanceService.Domain.Services;
using Xunit;

namespace Application.Tests
{
    public class DistanceServiceTests
    {
        private readonly IDistanceService _service;

        public DistanceServiceTests()
        {
            _service = new DistanceService.Application.Services.DistanceService();
        }

        [Theory]
        [InlineData(52.374030, 4.889690, 13.600000, 100.716700, 9202710.0370562151)]
        [InlineData(13.600000, 100.716700, 55.9739, 37.4141, 7110346.2060027458)]
        [InlineData(52.374030, 4.889690, 38.9534, -77.4477, 6211218.6688683508)]
        [InlineData(52.374030, 4.889690, 52.374030, 4.889690, 0)]
        public void GetDistanceInMeters_SetCoords_ReturnCorrectResult(double lat1, double lon1, double lat2,
            double lon2, double expectedResult)
        {
            // arrange
            // act
            var result = _service.GetDistanceInMeters(lat1, lon1, lat2, lon2);

            // assert
            Assert.True(Equals(result, expectedResult));
        }

        [Theory]
        [InlineData(20000, 12.427424)]
        [InlineData(0, 0)]
        public void ToMiles_SetMeters_ReturnCorrectResult(double meters, double expectedResult)
        {
            // arrange
            // act
            var result = _service.ToMiles(meters);

            // assert
            Assert.True(Equals(result, expectedResult));
        }

        [Fact]
        public void ToMiles_SetIncorrectValue_ThrowException()
        {
            // arrange
            var meters = -1;

            // act

            // assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.ToMiles(meters));
        }
    }
}
